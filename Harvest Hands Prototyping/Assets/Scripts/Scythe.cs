using UnityEngine;
using System.Collections;


public class Scythe : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string harvestSound = "event:/Priority/Harvesting";
    [FMODUnity.EventRef]
    public string harvestDeadSound = "event:/Priority/HarvestDead";

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
                    if (plant.isAlive)
                        FMODUnity.RuntimeManager.PlayOneShot(harvestSound, col.transform.position);
                    else
                        FMODUnity.RuntimeManager.PlayOneShot(harvestDeadSound, col.transform.position);

                    plant.CmdHarvest();
                    GameObject leaffall = (GameObject)Instantiate(plant.leafFallParticles, plant.transform.position, plant.transform.rotation);
                    Destroy(leaffall, plant.particlePlayDuration);
                    //Debug.Log("Calling cmdharvest");

                
                }
            }
        }         
    } 

}
