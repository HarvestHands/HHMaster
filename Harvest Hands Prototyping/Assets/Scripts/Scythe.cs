using UnityEngine;
using System.Collections;


public class Scythe : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string harvestSound = "event:/Priority/Harvesting";

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {    
        if (col.gameObject.CompareTag("Plant"))
        {
            Plantscript plant = col.gameObject.GetComponent<Plantscript>();
            {
                if (plant.ReadyToHarvest)
                {
                    plant.CmdHarvest();
                    GameObject leaffall = (GameObject)Instantiate(plant.leafFallParticles, plant.transform.position, plant.transform.rotation);
                    Destroy(leaffall, plant.particlePlayDuration);
                    //Play Sound
                    FMODUnity.RuntimeManager.PlayOneShot(harvestSound, col.transform.position);
                    //Debug.Log("Calling cmdharvest");
                }
            }
        }         
    } 

}
