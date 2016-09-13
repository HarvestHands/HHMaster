using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance = null;
    public bool LoadedLevel = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

	public void SetLoadedLevelTrue()
    {
        LoadedLevel = true;
    }

    public void SetLoadedLevelFalse()
    {
        LoadedLevel = false;
    }

    public GameObject bucketPrefab;
    public GameObject scythePrefab;
    public GameObject catapultPrefab;
    public GameObject mushroomSpawnerPrefab;
    public GameObject mushroomPrefab;
    public GameObject producePrefab;
    public GameObject soilPrefab;
    public GameObject shredderPrefab;
    public GameObject seedPrefab;
    public GameObject debrisPrefab;

    public List<GameObject> plantPrefabs;
    public List<GameObject> seedPrefabs;
    public List<GameObject> mushroomPrefabs;

}
