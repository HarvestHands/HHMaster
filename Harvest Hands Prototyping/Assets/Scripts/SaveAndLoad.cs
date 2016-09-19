using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;

public class SaveAndLoad : NetworkBehaviour
{
    public static SavedDataList localData;

    public delegate void SaveDelegate();
    public static event SaveDelegate SaveEvent;
    

    void OnLevelWasLoaded(int levelLoaded)
    {
        

    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log("LevelManager.instance = " + LevelManager.instance);
	    Debug.Log("OnLevelWasLoaded - Loaded a level = " + LevelManager.instance.LoadedLevel);

        if (LevelManager.instance.LoadedLevel)
        {
            LoadData();

            FireLoadEvent();
            Debug.Log("SavedGameManager.Count = " + localData.savedGameManager.Count);
            Debug.Log("SavedBuckets.Count = " + localData.savedBuckets.Count);
            Debug.Log("SavedScythe.Count = " + localData.savedScythe.Count);
            Debug.Log("SavedCatapult.Count = " + localData.savedCatapult.Count);
            Debug.Log("SavedMushroomSpawner.Count = " + localData.savedMushroomSpawner.Count);
            Debug.Log("SavedMushroom.Count = " + localData.savedMushroom.Count);
            Debug.Log("SavedProduce.Count = " + localData.savedProduce.Count);
            Debug.Log("SavedSoil.Count = " + localData.savedSoil.Count);
            Debug.Log("SavedShredder.Count = " + localData.savedShredder.Count);
            Debug.Log("SavedSeed.Count = " + localData.savedSeed.Count);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Firing Save Event! ");
            SaveData();
            Debug.Log("Save Event Finished! ");
        }
    }
	

    public void FireSaveEvent()
    {
        localData = new SavedDataList();

        localData.savedGameManager = new List<SavedGameManager>();
        localData.savedBuckets = new List<SavedBucket>();
        localData.savedScythe = new List<SavedScythe>();
        localData.savedCatapult = new List<SavedCatapult>();
        //localData.savedCatapultCrate = new List<SavedCatapultCrate>();
        localData.savedMushroomSpawner = new List<SavedMushroomSpawner>();
        localData.savedMushroom = new List<SavedMushroom>();   
        localData.savedProduce = new List<SavedProduce>();     
        localData.savedSoil = new List<SavedSoil>(); 
        localData.savedShredder = new List<SavedShredder>();
        localData.savedSeed = new List<SavedSeed>();
        

        
        SaveEvent();

        Debug.Log("SavedGameManager.Count = " + localData.savedGameManager.Count);
        Debug.Log("SavedBuckets.Count = " + localData.savedBuckets.Count);
        Debug.Log("SavedScythe.Count = " + localData.savedScythe.Count);
        Debug.Log("SavedCatapult.Count = " + localData.savedCatapult.Count);
        Debug.Log("SavedMushroomSpawner.Count = " + localData.savedMushroomSpawner.Count);
        Debug.Log("SavedMushroom.Count = " + localData.savedMushroom.Count);
        Debug.Log("SavedProduce.Count = " + localData.savedProduce.Count);
        Debug.Log("SavedSoil.Count = " + localData.savedSoil.Count);
        Debug.Log("SavedShredder.Count = " + localData.savedShredder.Count);
        Debug.Log("SavedSeed.Count = " + localData.savedSeed.Count);

    }

    public void FireLoadEvent()
    {
        //Load Data from Disk
        LoadData();
        //Remove default stuff
        RemoveDefaultObjects();
        //Create Loaded objects
        GenerateLoadedObjects();

    }

    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        FireSaveEvent();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");
        FileStream saveObjects = File.Create("Saves/saveObjects.binary");

        //localData = new GameObject();

        formatter.Serialize(saveFile, localData);
        //formatter.Serialize(saveObjects, saveList);

        saveFile.Close();
        saveObjects.Close();
    }


    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
        FileStream saveObjects = File.Open("Saves/saveObjects.binary", FileMode.Open);

        localData = (SavedDataList)formatter.Deserialize(saveFile);
        //savedLists = (List<SavedDroppableList>)formatter.Deserialize(saveObjects);

        saveFile.Close();
        saveObjects.Close();
    }


    public void RemoveDefaultObjects()
    {
        Debug.Log("Removing Default Objects!");
        //Remove default stuff
        //GameObject gameManager = FindObjectOfType<DayNightController>().gameObject; //Not deleted
        Water[] buckets = FindObjectsOfType<Water>();
        foreach (Water water in buckets)
        {
            Destroy(water.gameObject);
        }

        Scythe[] scythes = FindObjectsOfType<Scythe>();
        foreach (Scythe scythe in scythes)
        {
            Destroy(scythe.gameObject);
        }

        //StorageCatapult[] catapults = FindObjectsOfType<StorageCatapult>(); //just rewrite?
        //catapultcrates

        MushroomSpawner[] mushroomSpawners = FindObjectsOfType<MushroomSpawner>();
        foreach (MushroomSpawner mushroomSpawner in mushroomSpawners)
        {
            Destroy(mushroomSpawner.gameObject);
        }

        //Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();     //gets removed in produce
        PlantProduce[] produce = FindObjectsOfType<PlantProduce>();
        foreach (PlantProduce producei in produce)
        {
            Destroy(producei.gameObject);
        }

        SoilScript[] soils = FindObjectsOfType<SoilScript>();
        foreach (SoilScript soil in soils)
        {
            Destroy(soil.gameObject);
        }

        Shredder[] shredders = FindObjectsOfType<Shredder>();
        foreach (Shredder shredder in shredders)
        {
            Destroy(shredder.gameObject);
        }

        SeedScript[] seeds = FindObjectsOfType<SeedScript>();
        foreach (SeedScript seed in seeds)
        {
            Destroy(seed.gameObject);
        }
    }

    public void GenerateLoadedObjects()
    {
        Debug.Log("Generating Loaded Objects!");
        //Load GameManager
        foreach (SavedGameManager manager in localData.savedGameManager)
        {
            SaveAndLoad.LoadGameManager(manager);
        }
        //Load Buckets
        foreach (SavedBucket bucket in localData.savedBuckets)
        {
            SaveAndLoad.LoadBucket(bucket);
        }
        //Load Scythe
        foreach (SavedScythe scythe in localData.savedScythe)
        {
            SaveAndLoad.LoadScythe(scythe);
        }
        //Load Catapult
        foreach (SavedCatapult catapult in localData.savedCatapult)
        {
            SaveAndLoad.LoadCatapult(catapult);
        }
        //Load Mushroom Spawners
        foreach (SavedMushroomSpawner spawner in localData.savedMushroomSpawner)
        {
            SaveAndLoad.LoadMushroomSpawner(spawner);
        }
        //Load Mushrooms
        foreach (SavedMushroom mushroom in localData.savedMushroom)
        {
            SaveAndLoad.LoadMushroom(mushroom);
        }
        //Load Produce
        foreach (SavedProduce produce in localData.savedProduce)
        {
            SaveAndLoad.LoadProduce(produce);
        }
        //Load Soil
        foreach (SavedSoil soil in localData.savedSoil)
        {
            SaveAndLoad.LoadSoil(soil);
        }
        //Load Shredder
        foreach (SavedShredder shredder in localData.savedShredder)
        {
            SaveAndLoad.LoadShredder(shredder);
        }
        //Load Seed
        foreach (SavedSeed seed in localData.savedSeed)
        {
            SaveAndLoad.LoadSeed(seed);
        }
        //Load Debris
        //foreach (SavedDebris debris in localData.savedDebris)
        //{
        //    SaveAndLoad.LoadDebris(debris);
        //}
    }

    public static void LoadGameManager(SavedGameManager GM)
    {
        GameObject GameManager = FindObjectOfType<DayNightController>().gameObject;
        DayNightController DNC = GameManager.GetComponent<DayNightController>();
        DNC.ingameDay = GM.ingameDay;
        DNC.currentTimeOfDay = GM.currentTimeOfDay;
        DNC.nightTimeCheckDone = GM.dayNightCheckDone;

        BankScript bank = GameManager.GetComponent<BankScript>();
        bank.Score = GM.score;
        
    }

    public static void LoadBucket(SavedBucket bucket)
    {
        GameObject newBucket = Instantiate(LevelManager.instance.bucketPrefab);
        newBucket.transform.position = new Vector3(bucket.PosX, bucket.PosY, bucket.PosZ);
        newBucket.GetComponent<Water>().waterlevel = bucket.waterLevel;
        NetworkServer.Spawn(newBucket);
    }
    
    public static void LoadScythe(SavedScythe scythe)
    {
        GameObject newScythe = Instantiate(LevelManager.instance.scythePrefab);
        newScythe.transform.position = new Vector3(scythe.PosX, scythe.PosY, scythe.PosZ);
        NetworkServer.Spawn(newScythe);
    }

    public static void LoadCatapult(SavedCatapult catapult)
    {
        GameObject newCatapult = Instantiate(LevelManager.instance.catapultPrefab);
        newCatapult.transform.position = new Vector3(catapult.PosX, catapult.PosY, catapult.PosZ);
        newCatapult.GetComponent<StorageCatapult>().loadedObjects = catapult.loadedObjects;
        NetworkServer.Spawn(newCatapult);
    }

    public static void LoadMushroomSpawner(SavedMushroomSpawner spawner)
    {
        GameObject newSpawner = Instantiate(LevelManager.instance.mushroomSpawnerPrefab);
        newSpawner.transform.position = new Vector3(spawner.PosX, spawner.PosY, spawner.PosZ);
        newSpawner.GetComponent<MushroomSpawner>().canSpawn = spawner.canSpawn;
        //if mushroom != null 
        if (!spawner.canSpawn)
        {
            //spawn mushroom
            GameObject newMushroom = null;
            foreach (GameObject MushType in LevelManager.instance.mushroomPrefabs)
            {
                if (spawner.spawnedMushroomName == MushType.GetComponent<PlantProduce>().produceName)
                {
                    newMushroom = Instantiate(MushType);
                    break;
                }
            }
            if (newMushroom == null)
            {
                Debug.Log("Tried to load mushrrom but did not match any LevelManager.MushroomPrefabs");
                return;
            }
            newMushroom.transform.position = newSpawner.transform.position;
        }
        NetworkServer.Spawn(newSpawner);
    }

    public static void LoadMushroom(SavedMushroom mushroom)
    {
        GameObject newMushroom = Instantiate(LevelManager.instance.mushroomPrefab);
        newMushroom.transform.position = new Vector3(mushroom.PosX, mushroom.PosY, mushroom.PosZ);
        PlantProduce mushroomPlant = newMushroom.GetComponent<PlantProduce>();
        mushroomPlant.produceName = mushroom.produceName;
        mushroomPlant.score = mushroom.scoure;
        mushroomPlant.ProduceAmount = mushroom.produceAmount;
        NetworkServer.Spawn(newMushroom);
    }

    public static void LoadProduce(SavedProduce produce)
    {
        GameObject newProduce = Instantiate(LevelManager.instance.producePrefab);
        newProduce.transform.position = new Vector3(produce.PosX, produce.PosY, produce.PosZ);
        PlantProduce produceScript = newProduce.GetComponent<PlantProduce>();
        produceScript.produceName = produce.produceName;
        produceScript.score = produce.scoure;
        produceScript.ProduceAmount = produce.produceAmount;
        NetworkServer.Spawn(newProduce);
    }

    public static void LoadSoil(SavedSoil soil)
    {
        GameObject newsoil = Instantiate(LevelManager.instance.soilPrefab);
        newsoil.transform.position = new Vector3(soil.PosX, soil.PosY, soil.PosZ);
       
        //if occupied != false 
        if (soil.occupied)
        {
            //spawn plant
            newsoil.GetComponent<SoilScript>().CreatePlantFromData(soil.plantedPlant);
        }
        NetworkServer.Spawn(newsoil);
    }

    public static void LoadShredder(SavedShredder shredder)
    {
        GameObject newshredder = Instantiate(LevelManager.instance.shredderPrefab);
        newshredder.transform.position = new Vector3(shredder.PosX, shredder.PosY, shredder.PosZ);
        newshredder.GetComponent<Shredder>().tier = shredder.tier;
        NetworkServer.Spawn(newshredder);
    }

    public static void LoadSeed(SavedSeed seed)
    {
        GameObject newSeed = null;
        foreach (GameObject seedType in LevelManager.instance.seedPrefabs)
        {
            if (seed.seedName == seedType.GetComponent<SeedScript>().plantPrefab.GetComponent<Plantscript>().plantName)
            {
                newSeed = Instantiate(seedType);
                break;
            }
        }
        if (newSeed == null)
        {
            Debug.Log("Tried to load seed but did not match any LevelManager.SeedTypes");
            return;
        }

        newSeed.transform.position = new Vector3(seed.PosX, seed.PosY, seed.PosZ);
        newSeed.GetComponent<SeedScript>().NumberOfSeeds = seed.seedCount;
        NetworkServer.Spawn(newSeed);
    }

    public static void LoadDebris(SavedDebris debris)
    {

    }

    //localData.savedGameManager
    //localData.savedBuckets
    //localData.savedScythe
    //localData.savedCatapult
    ////localData.savedCatapultCrate 
    //localData.savedMushroomSpawner
    //localData.savedMushroom
    //localData.savedProduce
    //localData.savedSoil
    //localData.savedShredder
    //localData.savedSeed
}
