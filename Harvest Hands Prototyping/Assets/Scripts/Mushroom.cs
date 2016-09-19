using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mushroom : NetworkBehaviour
{
    //public int score = 3;

    [HideInInspector]
    public GameObject mushroomSpawner;
    
    	// Use this for initialization
	void Awake ()
    {
        if (mushroomSpawner != null)
        {
            GetComponent<Pickupable>().onPickedUp += WasPickedUp;
            //Debug.Log(name);
        }
        
        else
        {
            //Debug.Log("Hehe xd");
            return;
        }

    }

    void WasPickedUp()
    {
        Debug.Log("Mushroom Picked Up!", gameObject);
        if (mushroomSpawner != null)
        {
            CmdNullSpawner(mushroomSpawner.GetComponent<NetworkInstanceId>());
            mushroomSpawner.GetComponent<MushroomSpawner>().canSpawn = true;
        }
    }

    [Command]
    void CmdNullSpawner(NetworkInstanceId id)
    {
        MushroomSpawner spawner =  NetworkServer.FindLocalObject(id).GetComponent<MushroomSpawner>();
        spawner.canSpawn = true;
        mushroomSpawner = null;
        GetComponent<PlantProduce>().CmdAddSaveEvent();
    }

    public void SaveFunction(object sender, string args)
    {
        if (mushroomSpawner != null)
            return;
        PlantProduce produce = GetComponent<PlantProduce>();

        SavedProduce savedProduce = new SavedProduce();
        savedProduce.PosX = transform.position.x;
        savedProduce.PosY = transform.position.y;
        savedProduce.PosZ = transform.position.z;
        savedProduce.produceName = produce.produceName;
        savedProduce.produceAmount = produce.ProduceAmount;
        savedProduce.scoure = produce.score;

        SaveAndLoad.localData.savedProduce.Add(savedProduce);
    }

}

