using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SeedScript : NetworkBehaviour
{

    [SyncVar]
    public int NumberOfSeeds = 1;
    [Tooltip("What the seed plants.")]
    public GameObject plantPrefab;
    
	// Use this for initialization
	void Start ()
    {
        SaveAndLoad.SaveEvent += SaveFunction;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void OnDestroy()
    {
        if (GetComponent<Mushroom>() == null)
            SaveAndLoad.SaveEvent -= SaveFunction;
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Soil"))
        {
            SoilScript soil = col.GetComponent<SoilScript>();
            if (soil.occupied == false)
            {
                soil.CmdPlantSeed(plantPrefab, plantPrefab.GetComponent<Plantscript>().currentPlantState); // pass in type of seed?
                NumberOfSeeds--;
                soil.occupied = true;
                if (NumberOfSeeds < 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SaveFunction()
    {
        SavedSeed seed = new SavedSeed();
        seed.PosX = transform.position.x;
        seed.PosY = transform.position.y;
        seed.PosZ = transform.position.z;
        seed.seedName = plantPrefab.GetComponent<Plantscript>().plantName;
        seed.seedCount = NumberOfSeeds;

        SaveAndLoad.localData.savedSeed.Add(seed);
    }

}
