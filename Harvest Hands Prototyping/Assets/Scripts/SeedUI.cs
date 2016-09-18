using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SeedUI : MonoBehaviour {


     Text SeedNumber;
     Text SeedType;


	// Use this for initialization
	void Start () {

        SeedNumber =  this.GetComponentsInChildren<Text>()[0]; //("SBAMT");
        SeedType = this.GetComponentsInChildren<Text>()[1]; 

	}
	
	// Update is called once per frame
	void Update () {


        var ChosenSeed = this.GetComponent<SeedScript>();

        //NumberText.text = ChosenSeed.NumberOfSeeds.ToString();

        SeedNumber.text = ChosenSeed.NumberOfSeeds.ToString();
    //    SeedType.text = ChosenSeed.plantPrefab.GetComponent<Plantscript>().currentPlantType.ToString();


        SeedType.text = ChosenSeed.plantPrefab.GetComponent<Plantscript>().plantName;
	}
}
