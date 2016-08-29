using UnityEngine;
using System.Collections;


public class Scythe : MonoBehaviour
{
    public bool isCutting = false;
    public BoxCollider CutArea;

    void Start()
    {
        if (!isCutting)
        {
            CutArea.enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (isCutting)
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
                        Debug.Log("Calling cmdharvest");
                    }
                }
            }
        }
    }

    //[Command]
    public void ActivateCutting()
    {
        isCutting = true;
        CutArea.enabled = true;
    }

    public void DeactiveCutting()
    {
        isCutting = false;
        CutArea.enabled = false;
    }

    


}
