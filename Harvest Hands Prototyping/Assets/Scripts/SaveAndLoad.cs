using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;

public class SaveAndLoad : NetworkBehaviour
{
    public static SavedDataList localData;

    public delegate void SaveDelegate(object sender, string strng);//, Eventargs args)
    public static event SaveDelegate SaveEvent;

    public GameObject BucketPrefab;

    void OnLevelWasLoaded()
    {
        Debug.Log("OnLevelWasLoaded - Loaded a level = " + LevelManager.instance.LoadedLevel);

        if (LevelManager.instance.LoadedLevel)
        {
            FireLoadEvent();
        }

    }

    // Use this for initialization
    void Start ()
    {
	
	}
	

    public void FireSaveEvent()
    {
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
        

        if (SaveEvent != null)
            SaveEvent(null, null);
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
