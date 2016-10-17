using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProduceUI : MonoBehaviour {

    public Font textFont;

    public Text ProduceNumber;
    public Text ProduceType;


    // Use this for initialization
    void Start()
    {

        ProduceNumber = this.GetComponentsInChildren<Text>()[0]; //("SBAMT");
        ProduceType = this.GetComponentsInChildren<Text>()[1];

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
    void Update()
    {


        var ChosenSeed = this.GetComponent<PlantProduce>();

        //NumberText.text = ChosenSeed.NumberOfSeeds.ToString();

        ProduceNumber.text = ChosenSeed.ProduceAmount.ToString();
        //    SeedType.text = ChosenSeed.plantPrefab.GetComponent<Plantscript>().currentPlantType.ToString();


        ProduceType.text = ChosenSeed.produceName;
    }
}
