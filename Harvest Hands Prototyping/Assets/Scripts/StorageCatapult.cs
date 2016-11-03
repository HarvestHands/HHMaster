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
    [HideInInspector]
    public BankScript farmbank;

    public float scoreMultiplier = 1;
    public Text priceText;
    [HideInInspector]
    public int expectedIncome = 0;

    public List<GameObject> catapultCrates;
    public List<GameObject> loadedObjects;
    public int maxCrates = 8;

    public GameObject Spaghetti;
    public Transform SpaghettiSpawnPoint;
<<<<<<< HEAD
    public List<Transform> SpaghettiSpawnPoints;
=======
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e
    public float SpaghettiForce;
    public float SpaghettiDuration;
    [FMODUnity.EventRef]
    public string launchSpaghettiSound = "event:/Done/Gold income";


    void Start()
    {
        loadedObjects = new List<GameObject>();
    }

    // Use this for initialization
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager.GetComponent<DayNightController>().storageCatapults.Add(this);
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
            GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().Drop();
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
        if (loadedObjects.Count > 0)
            LaunchSpaghetti();

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
        if (priceText != null)
            priceText.text = "$" + expectedIncome.ToString();
    }

    public void LaunchSpaghetti()
    {
        Debug.Log("Inside LaunchSpaghetti Catapult");
<<<<<<< HEAD
        for (int i = 0; i < loadedObjects.Count; ++i)
        {
            GameObject newSpawn = (GameObject)Instantiate(Spaghetti, SpaghettiSpawnPoints[i].position, SpaghettiSpawnPoints[i].rotation);
            newSpawn.GetComponent<Rigidbody>().AddForce(SpaghettiSpawnPoints[i].transform.forward * SpaghettiForce, ForceMode.Impulse);
            Destroy(newSpawn, SpaghettiDuration);
        }


        //GameObject newSpawn = (GameObject)Instantiate(Spaghetti, SpaghettiSpawnPoint.position, SpaghettiSpawnPoint.rotation);
        //newSpawn.GetComponent<Rigidbody>().AddForce(SpaghettiSpawnPoint.transform.forward * SpaghettiForce, ForceMode.Impulse);
        //Destroy(newSpawn, SpaghettiDuration);
=======
        GameObject newSpawn = (GameObject)Instantiate(Spaghetti, SpaghettiSpawnPoint.position, SpaghettiSpawnPoint.rotation);
        newSpawn.GetComponent<Rigidbody>().AddForce(SpaghettiSpawnPoint.transform.forward * SpaghettiForce, ForceMode.Impulse);
        Destroy(newSpawn, SpaghettiDuration);
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e

        //play sound
        FMODUnity.RuntimeManager.PlayOneShot(launchSpaghettiSound, transform.position);
    }
}
