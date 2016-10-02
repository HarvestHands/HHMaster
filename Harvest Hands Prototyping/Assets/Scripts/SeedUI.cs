using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeedUI : MonoBehaviour {

	public Font textFont;

	public Text SeedNumber;
	public Text SeedType;


	// Use this for initialization
	void Start () {

        SeedNumber =  this.GetComponentsInChildren<Text>()[0]; //("SBAMT");
        SeedType = this.GetComponentsInChildren<Text>()[1]; 

		/*SeedNumber.font = textFont;
		SeedNumber.fontSize = 18;
		SeedNumber.color = Color.yellow;
		SeedNumber.horizontalOverflow = HorizontalWrapMode.Overflow;
		SeedNumber.verticalOverflow = VerticalWrapMode.Overflow;
		SeedNumber.lineSpacing = 5;

		SeedType.font = textFont;
		SeedType.fontSize = 18;
		SeedType.color = Color.yellow;
		SeedType.horizontalOverflow = HorizontalWrapMode.Overflow;
		SeedType.verticalOverflow = VerticalWrapMode.Overflow;
		SeedType.lineSpacing = 5;*/
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
