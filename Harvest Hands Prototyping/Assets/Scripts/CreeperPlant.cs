using UnityEngine;
using System.Collections;

public class CreeperPlant : MonoBehaviour {

    Plantscript thisPlant;
    public string CreeperResourceName = "CreeperPlantPrefab";

    public float spreadToEmptyChance = 20;
    public float spreadToPlantChance = 20;

    private GameObject CreeperResource;

	// Use this for initialization
	void Start ()
    {
        thisPlant = GetComponent<Plantscript>();
        CreeperResource = Resources.Load<GameObject>(CreeperResourceName);
	}
	
	
    public void AttemptSpread()
    {
        foreach(SoilScript soil in GetComponentInParent<SoilScript>().neighbourSoil)
        {
            //if spreading to empty soil
            if (!soil.occupied)
            {
                //if spread successful
                float chance = Random.Range(0, 100);
                Debug.Log("Random Chance = " + chance + " - spread to empty");
                if (chance <= spreadToEmptyChance)
                {                    
                    soil.CmdPlantSeed(CreeperResource, Plantscript.PlantState.Growing);
                    soil.occupied = true;
                }
            }
            //if spreading to plant(that's not creeper) on soil
            else if (soil.GetComponentInChildren<Plantscript>().currentPlantType != Plantscript.PlantType.Creeper)
            {
                //if spread successful
                float chance = Random.Range(0, 100);
                Debug.Log("Random Chance = " + chance + " - spread to occupied");
                if (chance <= spreadToPlantChance)
                {
                    Debug.Log("Destroy - " + GetComponentInChildren<Plantscript>().gameObject);
                    Destroy(GetComponentInChildren<Plantscript>().gameObject);
                    soil.CmdPlantSeed(CreeperResource, Plantscript.PlantState.Growing);
                    soil.occupied = true;
                }
            }
        }
    }


}
