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

    public enum PlantType
    {
        Root,
        Berry,
        Creeper,
    }
        

    [System.Serializable]
    public class MeshState
    {
        public Mesh mesh;
        public Material material;
    }

    public string plantName = "plantname";

    public PlantType currentPlantType = PlantType.Root;
    public PlantState currentPlantState = PlantState.Sapling;
    
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
    [HideInInspector]
    public float currentDryStreak = 0;
    //counter for days which do not count towards growth
    [HideInInspector]
    public float dryDays = 0;

    [Header("Berry Bush Attributes")]
    [SyncVar]
    public int harvestsToRemove = 1;
    [Tooltip("Amount of days until plant is ready to harvest again")]
    public int daysBetweenHarvest = 2;
    [HideInInspector]
    public int daySinceLastHarvest = 100;

    [HideInInspector]
    public float dayPlanted;
    [Tooltip("Not currently used")]
    [HideInInspector]
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
       

    public new Renderer renderer;
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshFilter meshFilter;
        
    public ParticleSystemScript plantingParticles;
    public ParticleSystemScript leafFallParticleSystem;
    public GameObject plantedParticles;
    public GameObject leafFallParticles;
    public float particlePlayDuration = 1f;


    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer = GetComponentInChildren<Renderer>();

        if (isWatered)
        {
            renderer.material = wateredMaterial;
        }
        else
        {
            renderer.material = dryMaterial;
        }

        GameObject plantParticles = (GameObject)Instantiate(plantedParticles, transform.position, transform.rotation);
        Destroy(plantParticles, particlePlayDuration);

        //plantingParticles.PlayParticles();
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
        //var meshFilter = GetComponent<MeshFilter>();
        //var meshFilter = GetComponentInChildren<MeshFilter>();

        if (state == PlantState.Sapling)
        {
            meshRenderer.enabled = false;
            skinnedMeshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = true;
            skinnedMeshRenderer.enabled = false;
        }
        

        switch (state)
        {
            case PlantState.Sapling:
                {
                    meshFilter.mesh = sapling.mesh;
                    //renderer.material = sapling.material;
                    currentPlantState = PlantState.Sapling;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growing.mesh;
                    //renderer.material = growing.material;
                    currentPlantState = PlantState.Growing;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grown.mesh;
                    //renderer.material = grown.material;
                    currentPlantState = PlantState.Grown;
                }
                break;
            case PlantState.Dead:
                {
                    meshFilter.mesh = dead.mesh;
                    //renderer.material = dead.material;
                    currentPlantState = PlantState.Dead;
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
        if (!isAlive)
        {
            Destroy(gameObject);
            GetComponentInParent<SoilScript>().occupied = false;
            GetComponentInParent<SoilScript>().plantedPlant = null;
            GetComponent<BoxCollider>().enabled = false;
            return;
        }

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
            isAlive = false;
            GetComponentInParent<SoilScript>().occupied = false;
            GetComponentInParent<SoilScript>().plantedPlant = null;
            //destroy self
            Destroy(gameObject);
            //leafFallParticleSystem.CmdPlayParticles();

            //disable plant functionality while particles play
            //meshRenderer.enabled = false;
            //skinnedMeshRenderer.enabled = false;
            //GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            CmdSwapPlantGraphics(PlantState.Growing);
        }
        Debug.Log("PlantHarvested");
    }

    [Command]
    public void CmdSwapPlantGraphics(Plantscript.PlantState state)
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
        {
            skinnedMeshRenderer.material = wateredMaterial;
            renderer.material = wateredMaterial;
        }
        else
        {
            skinnedMeshRenderer.material = dryMaterial;
            renderer.material = dryMaterial;
        }
    }

    [ClientRpc]
    public void RpcSpawnLeafFall()
    {
        GameObject leaffall = (GameObject)Instantiate(leafFallParticles, transform.position, transform.rotation);
        Destroy(leaffall, particlePlayDuration);
    }



    
}
