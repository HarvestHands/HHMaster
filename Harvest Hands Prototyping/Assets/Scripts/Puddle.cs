using UnityEngine;
using System.Collections;

public class Puddle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        Destroy(this.gameObject, 5);



    }



    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Plant")
        {
            Debug.Log("Watered");
            Plantscript plant = col.gameObject.GetComponent<Plantscript>();
            if (plant.isAlive)
            {
                if (!plant.isWatered)
                {
                    plant.isWatered = true;                    
                    //plant.SwitchPlantState(Plantscript.PlantState.Growing);
                }
            }
            

            Destroy(this.gameObject);
        }




    }
}