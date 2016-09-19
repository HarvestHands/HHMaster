using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SoilScript : NetworkBehaviour {

    [SyncVar] public bool occupied = false;

    public GameObject plantPrefab;

    private DayNightController dayNightController;

    public float neighbourDetectionRadius = 1.0f;
    public SoilScript[] neighbourSoil;

    public GameObject plantedPlant;

    // Use this for initialization
    void Awake()
    {
        dayNightController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DayNightController>();
        GetComponent<MeshRenderer>().enabled = false;

        List<SoilScript> nearbySoil = new List<SoilScript>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighbourDetectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Soil"))
            {
                nearbySoil.Add(collider.gameObject.GetComponent<SoilScript>());
            }
        }
        neighbourSoil = nearbySoil.ToArray();
        SaveAndLoad.SaveEvent += SaveFunction;
    }

    void OnDestroy()
    {
        SaveAndLoad.SaveEvent -= SaveFunction;
    }

    // Update is called once per frame
    void Update () {
	
	}

    [Command]
    public void CmdPlantSeed(GameObject plant, Plantscript.PlantState stage)
    {
        //create plant
        GameObject myNewPlant = Instantiate(plant);
        myNewPlant.transform.parent = gameObject.transform;
        myNewPlant.transform.localPosition = plantPrefab.transform.position;
        //myNewPlant.transform.localScale = plantPrefab.transform.localScale;

        //set plant details
        Plantscript plantScript = myNewPlant.GetComponent<Plantscript>();
        plantScript.dayPlanted = dayNightController.ingameDay;
        //plantScript.TimeToGrow = 1;

        plantScript.parentNetId = netId;
        plantScript.currentPlantState = stage;
        plantScript.SwitchPlantState(stage);

        //Add plant to server
        NetworkServer.Spawn(myNewPlant);
        myNewPlant.GetComponent<Plantscript>().CmdSwapPlantGraphics(stage);

        plantedPlant = myNewPlant;
    }

    public void SaveFunction()
    {
        SavedSoil soil = new SavedSoil();
        soil.PosX = transform.position.x;
        soil.PosY = transform.position.y;
        soil.PosZ = transform.position.z;
        soil.occupied = occupied;

        if (plantedPlant != null)
        {
            Plantscript plant = plantedPlant.GetComponent<Plantscript>();
            if (plant != null)
            {
                soil.plantedPlant = new SavedPlant();
                soil.plantedPlant.plantState = plant.currentPlantState;
                soil.plantedPlant.plantType = plant.currentPlantType;
                soil.plantedPlant.plantName = plant.plantName;
                soil.plantedPlant.readyToHarvest = plant.ReadyToHarvest;
                soil.plantedPlant.isWatered = plant.isWatered;
                soil.plantedPlant.isAlive = plant.isAlive;
                soil.plantedPlant.timeToGrow = plant.TimeToGrow;
                soil.plantedPlant.dryDaysToDie = plant.dryDaysToDie;
                soil.plantedPlant.harvestsToRemove = plant.harvestsToRemove;
                soil.plantedPlant.daysBetweenHarvest = plant.daysBetweenHarvest;
                soil.plantedPlant.daySinceLastHarvest = plant.daySinceLastHarvest;
                soil.plantedPlant.dayPlanted = plant.dayPlanted;
            }
            else
                Debug.Log("Trying to save plantedPlant without plantscript");
        }
        else
        {
            soil.plantedPlant = null;
        }

        SaveAndLoad.localData.savedSoil.Add(soil);
    }


    public void CreatePlantFromData(SavedPlant plantToMake)
    {
        GameObject myNewPlant = null;
        foreach (GameObject plantType in LevelManager.instance.plantPrefabs)
        {
            if (plantToMake.plantName == plantType.GetComponent<Plantscript>().plantName)
            {
                myNewPlant = Instantiate(plantType);
                break;
            }
        }
        if (myNewPlant == null)
        {
            Debug.Log("Tried to load plant but did not match any LevelManager.PlantTypes");
            return;
        }

        Plantscript myNewPlantScript = myNewPlant.GetComponent<Plantscript>();
        myNewPlantScript.currentPlantState = plantToMake.plantState;
        //created with planttype
        myNewPlantScript.ReadyToHarvest = plantToMake.readyToHarvest;
        myNewPlantScript.isWatered = plantToMake.isWatered;
        myNewPlantScript.isAlive = plantToMake.isAlive;
        myNewPlantScript.TimeToGrow = plantToMake.timeToGrow;
        myNewPlantScript.dryDaysToDie = plantToMake.dryDaysToDie;
        myNewPlantScript.currentDryStreak = plantToMake.currentDryStreak;
        myNewPlantScript.dryDays = plantToMake.dryDays;
        myNewPlantScript.harvestsToRemove = plantToMake.harvestsToRemove;
        myNewPlantScript.daysBetweenHarvest = plantToMake.daysBetweenHarvest;
        myNewPlantScript.daySinceLastHarvest = plantToMake.daySinceLastHarvest;
        myNewPlantScript.dayPlanted = plantToMake.dayPlanted;
        myNewPlantScript.CmdSwapPlantGraphics(myNewPlantScript.currentPlantState);
        myNewPlantScript.parentNetId = netId;

        myNewPlant.transform.parent = gameObject.transform;
        myNewPlant.transform.localPosition = plantPrefab.transform.position;

        plantedPlant = myNewPlant;
    }

    //public class SavedPlant
    //{
    //    public Plantscript.PlantState plantState;
    //    public Plantscript.PlantType plantType;
    //    public string plantName;
    //    public bool readyToHarvest;
    //    public bool isWatered;
    //    public bool isAlive;
    //    public float timeToGrow;
    //    public float dryDaysToDie;
    //    public float currentDryStreak;
    //    public float dryDays;
    //    public int harvestsToRemove;
    //    public int daysBetweenHarvest;
    //    public int daySinceLastHarvest;
    //    public float dayPlanted;
    //}

}


