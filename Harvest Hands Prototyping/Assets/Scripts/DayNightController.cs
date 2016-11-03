﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class DayNightController : NetworkBehaviour
{
    [SyncVar] public int ingameDay = 0;
    [Tooltip("Real world second per in-game day")]
    public float secondsInDay = 120f;

    [Tooltip("0 (midnight), 0.25 (sunrise), 0.5 (midday), 0.75(sunset), 1 (midnight)")]
    //[Tooltip("0 (sunrise) - 1 (sunset)")]

    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [SyncVar] public float timeMulitplier = 1f;

    public float startDayAt = 0.25f;
    public float endDayAt = 0.75f;
    public float nightPauseLength = 5f;
    
    public Light sun;

    public Transform stars;
    Material sky;

    float sunInitialIntensity;

    private NetworkStartPosition[] spawnPoints;
    private ShopScript shop;
    public MushroomSpawner[] mushroomSpawners;
    public List<StorageCatapult> storageCatapults;
    //public StorageCatapult storageCatapult;


    public List<Mushroom> mushrooms;

    [HideInInspector]
    public bool bedUsed = false;


    [SerializeField]
    [Tooltip("Score lost per player that died")]
    int deathPenalty = 0;

    private bool nightTimeCheckDone = false;

    public int playerdeathcount;

    RaycastHit Hit;
    public float GrabDistance = 3.0f;

    // Use this for initialization
    void Start ()
    {
        sky = RenderSettings.skybox;
        sunInitialIntensity = sun.intensity;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        shop = FindObjectOfType<ShopScript>();
        mushroomSpawners = FindObjectsOfType<MushroomSpawner>();
        playerdeathcount = 0;



        //mushrooms = FindObjectsOfType<Mushroom>();
    }

    // Update is called once per frame
    void Update ()
    {
        //only run if server?
        //if (!isServer)
        //    return;
    //    PlayerDeathPenalty();



        //insta sells catapaults items
        //if (Input.GetMouseButtonDown(0))
        //{
        //
        //    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        //
        //    Physics.Raycast(ray, out Hit, GrabDistance);
        //
        //    if (Hit.collider.gameObject == GameObject.Find("CatapaultButton"))
        //    {
        //        foreach (StorageCatapult catapult in storageCatapults)
        //        {
        //            catapult.CmdEmptyCatapult();
        //        }
        //    }
        //}
        
        //Update Sun rotation according to time of day
        UpdateSun();
        UpdateStars();

        currentTimeOfDay += (Time.deltaTime / secondsInDay) * timeMulitplier;
        GetComponent<FogEffect>().UpdateFog(currentTimeOfDay);

        if (!isServer)
            return;

        //Check if the day is over
        //if (currentTimeOfDay >= 1)
        if (currentTimeOfDay >= endDayAt)
        {
            if (!nightTimeCheckDone)
            {
                nightTimeCheckDone = true;
                //CmdCheckPlayersSafe();

                //yield return new WaitForSeconds(10);
                //Invoke("RespawnDeadPlayers", nightPauseLength / 2);
                Invoke("CmdUpdateNightStuff", nightPauseLength / 2);
            }
        }


	}

    [Command]
    void CmdUpdateNightStuff()
    {
        ingameDay++;
        CmdCheckPlayersSafe();
        //RespawnDeadPlayers ();
        CmdUpdatePlants();
        //CmdSetTimeOfDayMorning();
        CmdIncrementDay();
        CmdUpdateMushroomSpawners();
        //storageCatapult.CmdEmptyCatapult();
        BankScript bank = GetComponent<BankScript>();
        int money = bank.Score;
        //Debug.Log("Money = " + money);
        foreach (StorageCatapult catapult in storageCatapults)
        {
            catapult.CmdEmptyCatapult();
        }
        if (money != bank.Score)
        {
            bank.RpcSpawnPriceText(bank.Score - money);
        }
        Debug.Log("Bank.Score = " + bank.Score);
        Invoke("CmdSetTimeOfDayMorning", nightPauseLength / 2);
    }

    void UpdateSun()
    {
        //float daylightTime = endDayAt - startDayAt;
        //float wholeDayTime = (1 / daylightTime) *  //* secondsInDay;
        //Debug.Log(wholeDayTime);

        //-90 so that sun rise is at 0.25 instead of 0
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        //sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        //set intensity to low during night
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        //Increase sunlight intensity over time at sunrise
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        //Fade out sunlight over time at sunset
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
            
        }

        //Set sun intensity
        sun.intensity = sunInitialIntensity * intensityMultiplier;

    }

    void UpdateStars()
    {
        if (stars != null)
            stars.transform.rotation = sun.transform.rotation;
    }
    
    [Command]
    void CmdUpdatePlants()
    {
        foreach(GameObject plant in GameObject.FindGameObjectsWithTag("Plant"))
        {
            if (!plant.GetComponent<Plantscript>())
                continue;

            Plantscript plantScript = plant.GetComponent<Plantscript>();
            //if watered
            if (plantScript.isWatered)
            {
                //If creeper plant, attempt spread
                if (plantScript.currentPlantType == Plantscript.PlantType.Creeper)
                {
                    if (plantScript.currentPlantState == Plantscript.PlantState.Growing || plantScript.currentPlantState == Plantscript.PlantState.Grown)
                        plantScript.GetComponent<CreeperPlant>().AttemptSpread();
                }
                plantScript.daySinceLastHarvest++;
                //if not grown yet
                if (!plantScript.ReadyToHarvest)
                {
                    plantScript.currentDryStreak = 0;
                    //if ready to grow
                    if (ingameDay >= plantScript.dayPlanted + plantScript.TimeToGrow + plantScript.dryDays)
                    {
                        plantScript.ReadyToHarvest = true;
                        RpcSwapPlantGraphics(plantScript.netId, Plantscript.PlantState.Grown);
                        plantScript.CmdSwapPlantMaterial(Plantscript.PlantStateMat.Grown);
                    }
                    else
                    {
                        plantScript.isWatered = false;
                        RpcSwapPlantGraphics(plantScript.netId, Plantscript.PlantState.Growing);
                        plantScript.CmdSwapPlantMaterial(Plantscript.PlantStateMat.Dry);
                    }
                }
            }
            //plant not watered
            else
            {
                plantScript.currentDryStreak++;
                plantScript.dryDays++;


                //plant dies
                if (plantScript.currentDryStreak >= plantScript.dryDaysToDie)
                {
                    plantScript.ReadyToHarvest = true;
                    plantScript.isAlive = false;
                    RpcSwapPlantGraphics(plantScript.netId, Plantscript.PlantState.Dead);
                    plantScript.CmdSwapPlantMaterial(Plantscript.PlantStateMat.Dead);
                }
            }

            //plantScript.isWatered = false;
        }
    }

    [ClientRpc]
    void RpcSwapPlantGraphics(NetworkInstanceId id, Plantscript.PlantState state)
    {
        var plant = ClientScene.FindLocalObject(id);
        if(plant == null)
        {
            Debug.LogError("Where is plant? ID: " + id.ToString());
            return;
        }

        var plantScript = plant.GetComponent<Plantscript>();
        plantScript.SwitchPlantState(state);
    }

    [Command]
    void CmdCheckPlayersSafe()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        int playersDead = 0;
        foreach (GameObject player in Players)
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 10;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 20;
            //player.GetComponent<DeathFade>().RpcFadeIn();
            if (!player.GetComponent<PlayerInventory>().isSafe)
            {
                int respawnIndex = Random.Range(0, spawnPoints.Length - 1);
                RpcRespawnPlayer(player.GetComponent<NetworkIdentity>().netId, spawnPoints[respawnIndex].transform.position);
                player.GetComponent<DeathFade>().CmdShowDeadText();
                playerdeathcount += 1;
                PlayerDeathPenalty();

                player.GetComponent<PlayerInventory>().RpcApplyDeathPenalty();
                playersDead++;
                player.GetComponent<DeathFade>().RpcSetShowDeathPenaltyImage(true);                
            }
            else
            {
                player.GetComponent<DeathFade>().RpcSetShowDeathPenaltyImage(false);
            }
        }
    }
        
    [ClientRpc]
    public void RpcRespawnPlayer(NetworkInstanceId id, Vector3 newPos)
    {
        GameObject player = ClientScene.FindLocalObject(id);
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().Respawn(newPos);
        //transform.position = newPos;
    }

    void RespawnDeadPlayers()
    {
        //Debug.Log("Respawning!");
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in Players)
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed = 10;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = 20;
            //player.GetComponent<DeathFade>().RpcFadeOut();
            //if player is NOT safe
            if (!player.GetComponent<PlayerInventory>().isSafe)
            {

                int respawnIndex = Random.Range(0, spawnPoints.Length - 1);
                //player.GetComponent<DeathFade>().RpcFadeOut(false);

                RpcRespawnPlayer(player.GetComponent<NetworkIdentity>().netId, spawnPoints[respawnIndex].transform.position);

                player.GetComponent<DeathFade>().CmdShowDeadText();
                playerdeathcount += 1;
                PlayerDeathPenalty();
            }
            else
            {
                //player.GetComponent<DeathFade>().RpcFadeOut(true);
            }
        }
    }


    public void PlayerDeathPenalty()
    {   
        Debug.Log("PLAYERDEATHTEST");
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in Players)
        {
            //default 10
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed /= 2;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed /= 2;

            Debug.Log(player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_WalkSpeed);
            if (player.GetComponent<StaffNo3>().ChosenObj != null)
                player.GetComponent<StaffNo3>().CmdDropped();
            player.GetComponent<StaffNo3>().Drop();
        }
    }

    [Command]
    void CmdSetTimeOfDayMorning()
    {
        Debug.Log("Inside CmdSetTimeOfDayMorning");
        currentTimeOfDay = startDayAt;
        nightTimeCheckDone = false;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<DeathFade>().CmdMorningFadeOut();
        }
        //RpcSyncTimeOfDay(currentTimeOfDay);
        
        //RpcSyncTimeOfDay(startDayAt);
    }

    [Command]
    void CmdIncrementDay()
    {
        //ingameDay++;
        //currentTimeOfDay = startDayAt;
		//RpcSyncTimeOfDay(currentTimeOfDay);  

        //bedUsed = false;
    }

    [ClientRpc]
    void RpcSyncTimeOfDay(float _timeOfDay)
    {
        currentTimeOfDay = _timeOfDay;
    }

    [Command]
    void CmdUpdateMushroomSpawners()
    {
        foreach(MushroomSpawner spawner in mushroomSpawners)
        {
            spawner.AttemptSpawn();
        }
    }


}
