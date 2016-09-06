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
    public override void OnStartClient()
    {
        base.OnStartClient();
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


}


