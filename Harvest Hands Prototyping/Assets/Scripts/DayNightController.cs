using UnityEngine;
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

    [SerializeField]
    [Tooltip("Score lost per player that died")]
    int deathPenalty = 0;

    private bool nightTimeCheckDone = false;
    

    
	// Use this for initialization
	void Start ()
    {
        sky = RenderSettings.skybox;
        sunInitialIntensity = sun.intensity;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        shop = FindObjectOfType<ShopScript>();
        mushroomSpawners = FindObjectsOfType<MushroomSpawner>();
    }

    // Update is called once per frame
    void Update ()
    {
        //only run if server?
        //if (!isServer)
        //    return;


        //Update Sun rotation according to time of day
        UpdateSun();
        UpdateStars();

        currentTimeOfDay += (Time.deltaTime / secondsInDay) * timeMulitplier;

        if (!isServer)
            return;

        //Check if the day is over
        //if (currentTimeOfDay >= 1)
        if (currentTimeOfDay >= endDayAt)
        {
            if (!nightTimeCheckDone)
            {
                nightTimeCheckDone = true;
                //currentTimeOfDay = 0;
                //currentTimeOfDay = startDayAt;
                //ingameDay++;
                //CmdUpdatePlants();
                CmdCheckPlayersSafe();

                Invoke("RespawnDeadPlayers", nightPauseLength / 2);
                Invoke("CmdUpdateNightStuff", nightPauseLength / 2);
                Invoke("CmdSetTimeOfDayMorning", nightPauseLength);
            }
        }


	}

    [Command]
    void CmdUpdateNightStuff()
    {
        ingameDay++;
        CmdUpdatePlants();
        CmdSetTimeOfDayMorning();
        CmdIncrementDay();
        CmdUpdateMushroomSpawners();
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
                    if (ingameDay >= plantScript.dayPlanted + plantScript.TimeToGrow - plantScript.dryDays)
                    {
                        plantScript.ReadyToHarvest = true;
                        RpcSwapPlantGraphics(plantScript.netId, Plantscript.PlantState.Grown);
                    }
                    else
                    {
                        plantScript.isWatered = false;
                        RpcSwapPlantGraphics(plantScript.netId, Plantscript.PlantState.Growing);
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
        

        foreach(GameObject player in Players)
        {
            player.GetComponent<DeathFade>().RpcFadeIn();
            if (!player.GetComponent<PlayerInventory>().isSafe)
            {
                playersDead++;
                //int respawnIndex = Random.Range(0, spawnPoints.Length -1);
                //player.transform.position = spawnPoints[respawnIndex].transform.position;
                //player.transform.rotation = spawnPoints[respawnIndex].transform.rotation;
                
            }
        }
        int scoreLost = deathPenalty * playersDead;
        //Debug.Log(shop.Score + " - " + scoreLost);
        shop.Score -= scoreLost;
        //Debug.Log(shop.Score);
        
        



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
            player.GetComponent<DeathFade>().RpcFadeOut();
            //if player is NOT safe
            if (!player.GetComponent<PlayerInventory>().isSafe)
            {
                int respawnIndex = Random.Range(0, spawnPoints.Length - 1);
                //player.transform.position = spawnPoints[respawnIndex].transform.position;
                //player.transform.rotation = spawnPoints[respawnIndex].transform.rotation;

                RpcRespawnPlayer(player.GetComponent<NetworkIdentity>().netId, spawnPoints[respawnIndex].transform.position);

                //player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().RpcRespawnPlayer(spawnPoints[respawnIndex].transform.position);

            }
        }
    }

    [Command]
    void CmdSetTimeOfDayMorning()
    {
        currentTimeOfDay = startDayAt;
        RpcSyncTimeOfDay(currentTimeOfDay);
    }

    [Command]
    void CmdIncrementDay()
    {
        //ingameDay++;
        currentTimeOfDay = startDayAt;
        nightTimeCheckDone = false;
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
