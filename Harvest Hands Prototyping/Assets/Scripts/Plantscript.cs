using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Plantscript : NetworkBehaviour
{

    public enum PlantState
    {
        Sapling,
        Growing,
        Grown,
        Dead,
    }

    [System.Serializable]
    public class MeshState
    {
        public Mesh mesh;
        public Material material;
    }

    [Header("Plant life")]

    [SyncVar]
    public bool ReadyToHarvest = false;
    [SyncVar (hook = "OnWateredChange")]
    public bool isWatered = false;
    [SyncVar]
    public bool isAlive = true;
    [SyncVar]
    public NetworkInstanceId parentNetId;

    [Tooltip("1.0 = 1 full day")]
    public float TimeToGrow;
    [Tooltip("1.0 = 1 full day")]
    public float dryDaysToDie = 2;
    [Tooltip("Current amount of day spent unwatered")]
    public float currentDryStreak = 0;
    //counter for days which do not count towards growth
    [HideInInspector]
    public float dryDays = 0;

    public float dayPlanted;
    [Tooltip("Not currently used")]
    public float timeOfDay;

    //[Header("Produce Attributes",[1])]
    public int minSeedsProduced = 1;
    public int maxSeedsProduced = 4;
    public GameObject plantProducePrefab;

    public Material HarvestMaterial;

    public MeshState sapling;
    public MeshState growing;
    public MeshState grown;
    public MeshState dead;

    public Material wateredMaterial;
    public Material dryMaterial;

    [Header("Berry Bush Attributes")]
    [SyncVar]
    public int harvestsToRemove = 1;
    [Tooltip("Amount of days until plant is ready to harvest again")]
    public int daysBetweenHarvest = 2;
    [HideInInspector]
    public int daySinceLastHarvest = 100;

    Renderer renderer;


    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        //if (ReadyToHarvest)
        //{
        //    GetComponent<Renderer>().material = HarvestMaterial;
        //}

        if (isWatered)
        {
            renderer.material = wateredMaterial;
        }
        else
        {
            renderer.material = dryMaterial;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
        transform.SetParent(parentObject.transform);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchPlantState(PlantState state)
    {
        var meshFilter = GetComponent<MeshFilter>();
        switch (state)
        {
            case PlantState.Sapling:
                {
                    meshFilter.mesh = sapling.mesh;
                    //renderer.material = sapling.material;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growing.mesh;
                    //renderer.material = growing.material;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grown.mesh;
                    //renderer.material = grown.material;
                }
                break;
            case PlantState.Dead:
                {
                    meshFilter.mesh = dead.mesh;
                    //renderer.material = dead.material;
                }
                break;
            default:
                Debug.LogError("U wot m8!?");
                break;
        }

        //if (isWatered)
        //{
        //    renderer.material = wateredMaterial;
        //}
        //else
        //{
        //    renderer.material = dryMaterial;
        //}
    }

    [Command]
    public void CmdHarvest()
    {
        if (isAlive && daySinceLastHarvest >= daysBetweenHarvest)
        {
            //create produce
            GameObject produce = Instantiate(plantProducePrefab);
            produce.GetComponent<PlantProduce>().ProduceAmount = Random.Range(minSeedsProduced, maxSeedsProduced);
            produce.transform.position = transform.position;
            //move produce out of ground
            produce.transform.position += new Vector3(0, 1, 0);

            //Spawn on server
            NetworkServer.Spawn(produce);

            harvestsToRemove--;
            ReadyToHarvest = false;
            daySinceLastHarvest = 0;
        }


        if (harvestsToRemove < 1)
        {

            GetComponentInParent<SoilScript>().occupied = false;
            //destroy self
            Destroy(gameObject);
        }
        else
        {
            CmdSwapPlantGraphics(PlantState.Growing);
        }
        Debug.Log("PlantHarvested");
    }

    [Command]
    void CmdSwapPlantGraphics(Plantscript.PlantState state)
    {
        RpcSwapPlantGraphics(state);
    }


    [ClientRpc]
    void RpcSwapPlantGraphics(Plantscript.PlantState state)
    {
        var plant = ClientScene.FindLocalObject(netId);
        if (plant == null)
        {
            Debug.LogError("Where is plant? ID: " + netId.ToString());
            return;
        }
        
        SwitchPlantState(state);
    }


    void OnWateredChange(bool watered)
    {
        if (watered)
            renderer.material = wateredMaterial;
        else
            renderer.material = dryMaterial;
    }

}
