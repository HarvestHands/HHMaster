using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    public Text priceText;
    private int expectedIncome = 0;

    public List<GameObject> catapultCrates;
    public List<GameObject> loadedObjects;
    public int maxCrates = 8;

    void Start()
    {
        loadedObjects = new List<GameObject>();
    }

    // Use this for initialization
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        farmbank = gameManager.GetComponent<BankScript>();
        foreach (GameObject crate in catapultCrates)
        {
            crate.SetActive(false);
        }
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
            if (loadedObjects.Count >= maxCrates)
                return;

            CmdAddCrate(col.GetComponent<NetworkIdentity>().netId);
        }
    }

    [Command]
    void CmdAddCrate(NetworkInstanceId id)
    {
        //Check if room for crate
        if (loadedObjects.Count < maxCrates)
        {
            GameObject crateObject = NetworkServer.FindLocalObject(id);
            RpcAddCrate(id);
        }
    }

    [ClientRpc]
    void RpcAddCrate(NetworkInstanceId id)
    {
        //Add crate to loadedObjects list
        PlantProduce produceObject = ClientScene.FindLocalObject(id).GetComponent<PlantProduce>();
        int crateIndex = loadedObjects.Count;
        loadedObjects.Add(produceObject.gameObject);

        //Set and active crate in catapult
        catapultCrates[crateIndex].SetActive(true);
        CatapultCrate crateScript = catapultCrates[crateIndex].GetComponent<CatapultCrate>();
        crateScript.nameText.text = produceObject.produceName;
        crateScript.amountText.text = produceObject.ProduceAmount.ToString();

        //Update sign
        expectedIncome += produceObject.ProduceAmount * produceObject.score;
        if (priceText != null)
            priceText.text = "$" + expectedIncome.ToString();

        //Deactive actual crate
        produceObject.gameObject.SetActive(false);                
    }

    [Command]
    public void CmdEmptyCatapult()
    {
        foreach(GameObject sellingObj in loadedObjects)
        {
            PlantProduce produce = sellingObj.GetComponent<PlantProduce>();
            farmbank.Score += produce.score * produce.ProduceAmount;
        }
        RpcEmptyCatapultLists();
    }

    [ClientRpc]
    void RpcEmptyCatapultLists()
    {
        foreach(GameObject crate in loadedObjects)
        {
            if (crate != null)
                Destroy(crate);
        }
        loadedObjects = new List<GameObject>();

        foreach (GameObject crate in catapultCrates)
        {
            crate.SetActive(false);
        }

        expectedIncome = 0;
        priceText.text = "$" + expectedIncome.ToString();
    }
}
