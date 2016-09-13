using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


[System.Serializable]
public class SavedDataList
{
    public List<SavedGameManager> savedGameManager = new List<SavedGameManager>();
    public List<SavedBucket> savedBuckets = new List<SavedBucket>();
    public List<SavedScythe> savedScythe = new List<SavedScythe>();
    public List<SavedCatapult> savedCatapult = new List<SavedCatapult>();
    //public List<SavedCatapultCrate> savedCatapultCrate = new List<SavedCatapultCrate>();
    public List<SavedMushroomSpawner> savedMushroomSpawner = new List<SavedMushroomSpawner>();          //MAKE THE SAVE FUNCTION
    public List<SavedMushroom> savedMushroom = new List<SavedMushroom>();      //Mushroom IS produce
    public List<SavedProduce> savedProduce = new List<SavedProduce>();         //Mushroom IS produce
    public List<SavedSoil> savedSoil = new List<SavedSoil>();   //SavedSoil has SavedPlant
    public List<SavedShredder> savedShredder = new List<SavedShredder>();
    public List<SavedSeed> savedSeed = new List<SavedSeed>();
    public List<Debris> savedDebris = new List<Debris>();
}

[System.Serializable]
public class SavedGameManager // Save is in BankScript
{
    //DayNightController
    public int ingameDay;
    public float currentTimeOfDay;
    public bool dayNightCheckDone;

    //Bank
    public int score;
}

[System.Serializable]
public class SavedBucket
{
    public float PosX, PosY, PosZ;
    public float waterLevel;
}

[System.Serializable]
public class SavedScythe
{
    public float PosX, PosY, PosZ;
}

[System.Serializable]
public class SavedCatapult                  //TODO
{
    public float PosX, PosY, PosZ;
    //public float expectedIncome;
    public List<GameObject> loadedObjects;
}

[System.Serializable]
public class SavedCatapultCrate
{
    public float PosX, PosY, PosZ;
}           //TODO IF NEEDED?

[System.Serializable]
public class SavedMushroomSpawner
{
    public float PosX, PosY, PosZ;
    public bool canSpawn;
    public string spawnedMushroomName;
    //list of chances and stuff?
}

[System.Serializable]
public class SavedMushroom
{
    public float PosX, PosY, PosZ;
    public string produceName;
    public int produceAmount, scoure;
}

[System.Serializable]
public class SavedProduce
{
    public float PosX, PosY, PosZ;
    public string produceName;
    public int produceAmount, scoure;
}

[System.Serializable]
public class SavedPlant
{
    public Plantscript.PlantState plantState;
    public Plantscript.PlantType plantType;
    public string plantName;
    public bool readyToHarvest;
    public bool isWatered;
    public bool isAlive;
    public float timeToGrow;
    public float dryDaysToDie;
    public float currentDryStreak;
    public float dryDays;
    public int harvestsToRemove;
    public int daysBetweenHarvest;
    public int daySinceLastHarvest;
    public float dayPlanted;
}

[System.Serializable]
public class SavedSoil
{
    public float PosX, PosY, PosZ;
    public bool occupied;
    public SavedPlant plantedPlant;
}

[System.Serializable]
public class SavedShredder
{
    public float PosX, PosY, PosZ;
    public int tier;
}

[System.Serializable]
public class SavedSeed
{
    public float PosX, PosY, PosZ;
    public string seedName;
    public int seedCount;
}

[System.Serializable]
public class SavedDebris
{
    public float PoxX, PosY, PosZ;
}







//