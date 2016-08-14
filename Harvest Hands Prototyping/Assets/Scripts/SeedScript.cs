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
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}    
   

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Soil"))
        {
            SoilScript soil = col.GetComponent<SoilScript>();
            if (soil.occupied == false)
            {
                soil.CmdPlantSeed(plantPrefab); // pass in type of seed?
                NumberOfSeeds--;
                soil.occupied = true;
                if (NumberOfSeeds < 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
