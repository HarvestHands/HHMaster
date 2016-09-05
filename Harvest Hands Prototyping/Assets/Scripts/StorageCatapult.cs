using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StorageCatapult : NetworkBehaviour
{
    [System.Serializable]
    public class CatapultStorageCrate
    {
        public string name;
        public float amount;
    }

    GameObject gameManager;
    //ShopScript shop;
    BankScript farmbank;

    public float scoreMultiplier = 1;
    public List<GameObject> loadedObjects;
    public int maxCrates = 8;

    

    // Use this for initialization
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        //shop = gameManager.GetComponent<ShopScript>();
        farmbank = gameManager.GetComponent<BankScript>();
        loadedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //TO DO CATAPULT        ---------------------------------------------------------------------------------------------------------
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Produce"))
        {
            //Make Command?
            PlantProduce produce = col.gameObject.GetComponent<PlantProduce>();
            //shop.Score += produce.score;
            farmbank.Score += produce.score;
            Destroy(produce.gameObject);
        }
        //else if (col.gameObject.CompareTag("Mushroom"))
        //{
        //    Mushroom mushroom = col.gameObject.GetComponent<Mushroom>();
        //    farmbank.Score += mushroom.score;
        //    Destroy(mushroom.gameObject);
        //}
    }

    [Command]
    void CmdAddCrate(NetworkInstanceId id)
    {
        if (loadedObjects.Count < maxCrates)
        {
            GameObject crateObject = NetworkServer.FindLocalObject(id);
            RpcAddCrate(id);
        }
    }

    [ClientRpc]
    void RpcAddCrate(NetworkInstanceId id)
    {
        GameObject crateObject = ClientScene.FindLocalObject(id);

    }
}
