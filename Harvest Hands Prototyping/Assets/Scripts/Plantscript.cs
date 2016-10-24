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
    public enum PlantStateMat
    {
        Dry,
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
    public PlantStateMat currentPlantStateMat = PlantStateMat.Dry;
    
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
    [Tooltip("Current amount of day spent unwatered")][HideInInspector]
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

    [Header("Produce Attributes")]
    public int minSeedsProduced = 1;
    public int maxSeedsProduced = 4;
    public GameObject plantProducePrefab;
    
    [Header("PlantState Meshes+Materials")]
    public Mesh saplingMesh;
    public Mesh growingMesh;
    public Mesh grownMesh;
    public Mesh deadMesh;

    public Material dryMaterial;
    public Material growingMaterial;
    public Material grownMaterial;
    public Material deadMaterial;


    public new Renderer renderer;
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshFilter meshFilter;
        
    [Header("Particle Effects")]
    public ParticleSystemScript plantingParticles;
    public ParticleSystemScript leafFallParticleSystem;
    public GameObject plantedParticles;
    public GameObject leafFallParticles;
    public float particlePlayDuration = 1f;

    [Header("Sound Effects")]
    [FMODUnity.EventRef]
    public string wateredSound = "event:/Done/Watering Plant";
    [FMODUnity.EventRef]
    public string harvestedSound = "event:/Priority/Harvesting";

    // Use this for initialization
    void Start()
    {
		transform.localRotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
        renderer = GetComponent<Renderer>();
        renderer = GetComponentInChildren<Renderer>();

        if (ReadyToHarvest)
            renderer.material = grownMaterial;
        else if (!isAlive)
            renderer.material = deadMaterial;
        else if (isWatered)
            renderer.material = growingMaterial;
        else
            renderer.material = dryMaterial;

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
                    meshFilter.mesh = saplingMesh;
                    //renderer.material = sapling.material;
                    currentPlantState = PlantState.Sapling;
                }
                break;
            case PlantState.Growing:
                {
                    meshFilter.mesh = growingMesh;
                    //renderer.material = growing.material;
                    currentPlantState = PlantState.Growing;
                }
                break;
            case PlantState.Grown:
                {
                    meshFilter.mesh = grownMesh;
                    //renderer.material = grown.material;
                    currentPlantState = PlantState.Grown;
                }
                break;
            case PlantState.Dead:
                {
                    meshFilter.mesh = deadMesh;
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
            CmdSwapPlantMaterial(PlantStateMat.Growing);
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

    [Command]
    public void CmdSwapPlantMaterial(Plantscript.PlantStateMat stateMat)
    {
        RpcSwapPlantMaterial(stateMat);
    }

    [ClientRpc]
    void RpcSwapPlantMaterial(Plantscript.PlantStateMat stateMat)
    {
        var plant = ClientScene.FindLocalObject(netId);
        if (plant == null)
        {
            Debug.LogError("Where is plant? ID: " + netId.ToString());
            return;
        }

        SwitchPlantMaterial(stateMat);
    }

    public void SwitchPlantMaterial(PlantStateMat stateMat)
    {
        switch (stateMat)
        {
            case PlantStateMat.Dead:
                {
                    renderer.material = deadMaterial;
                    currentPlantStateMat = PlantStateMat.Dead;
                }
                break;
            case PlantStateMat.Dry:
                {
                    renderer.material = dryMaterial;
                    currentPlantStateMat = PlantStateMat.Dry;
                }
                break;
            case PlantStateMat.Growing:
                {
                    renderer.material = growingMaterial;
                    currentPlantStateMat = PlantStateMat.Growing;
                }
                break;
            case PlantStateMat.Grown:
                {
                    renderer.material = grownMaterial;
                    currentPlantStateMat = PlantStateMat.Grown;
                }
                break;
            default:
                Debug.LogError("Trying to swap PlantStateMat to unknown state");
                break;
        }
    }

    void OnWateredChange(bool watered)
    {
        
        if (watered)
        {
            skinnedMeshRenderer.material = growingMaterial;
            renderer.material = growingMaterial;
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
