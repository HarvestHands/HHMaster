using UnityEngine;
using System.Collections;

public class Scythe : MonoBehaviour
{
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Plant"))
        {            
            Plantscript plant = col.gameObject.GetComponent<Plantscript>();
            {
                if (plant.ReadyToHarvest)
                {
                    plant.CmdHarvest();
                }
            }
        }
    }


}
