using UnityEngine;
using System.Collections;


public class Scythe : MonoBehaviour
{
    //public bool isCutting = false;
    //public BoxCollider CutArea;

    void Start()
    {
        SaveAndLoad.SaveEvent += SaveFunction;
    }
    void OnDestroy()
    {
        SaveAndLoad.SaveEvent -= SaveFunction;
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
                    Debug.Log("Calling cmdharvest");
                }
            }
        }       
    }

    public void SaveFunction(object sender, string args)
    {
        SavedScythe scythe = new SavedScythe();
        scythe.PosX = transform.position.x;
        scythe.PosY = transform.position.y;
        scythe.PosZ = transform.position.z;

        SaveAndLoad.localData.savedScythe.Add(scythe);
    }




}
