using UnityEngine;
using System.Collections;

public class MergerV2 : MonoBehaviour
{
    public enum MergeType
    {
        NONE = -1,
        SEED = 0,
        PRODUCE = 1,
    }

    public MergeType mergeType = MergeType.NONE;
    public int stackLimit = 20;
    public float attractionRadius = 4f;
    public float attractionStrength = 0.01f;
    public float mergeRadius = 1;
    public GameObject mergeParticles;
    [Tooltip("Destroy particle objects after this amount of time")]
    public float particleLifeTime = 4f;

    private Transform closestMerge;
    


	// Use this for initialization
	void Awake ()
    {
        //StartCoroutine(FindMergeTarget());
        InvokeRepeating("FindMergeTarget", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //FindMergeTarget();

        if (closestMerge != null)
        {
            transform.position = Vector3.Lerp(transform.position, closestMerge.transform.position, attractionStrength);

            //Check if in range to actually merge
            if (Vector3.Distance(transform.position, closestMerge.position) <= mergeRadius)
            {       
                Merge(closestMerge);                   
            }
        }














        /*
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.tag == "Produce")
        {

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Produce").Length; i++)
        {
            // Debug.Log(GameObject.FindGameObjectsWithTag("ShopItemA").Length);


            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.transform.position, GameObject.FindGameObjectsWithTag("Produce")[i].transform.position) <= 1 && GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj != GameObject.FindGameObjectsWithTag("Produce")[i]) // > 2 && SpawnA == true)
            {

                Instantiate(Resources.Load("PlantProduceMed"), GameObject.FindGameObjectsWithTag("Produce")[i].transform.position, GameObject.FindGameObjectsWithTag("Produce")[i].transform.rotation);


                Destroy(GameObject.FindGameObjectsWithTag("Produce")[i]);
                Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj);

            }
        }
       }
       */    
	}

    //IEnumerator FindMergeTarget()
    void FindMergeTarget()
    {
        closestMerge = null;
        //Debug.Log("Inside FindMergeTarget");
        if (mergeType == MergeType.NONE)
            return;
            //yield return new WaitForSeconds(0.5f);
        
        //if full, return
        if (mergeType == MergeType.SEED)
        {
            if (GetComponent<SeedScript>().NumberOfSeeds >= stackLimit)
            {
                return;
            }
        }
        else if (mergeType == MergeType.PRODUCE)
        {
            if (GetComponent<PlantProduce>().ProduceAmount >= stackLimit)
            {
                return;
            }
        }

        float closestDist = Mathf.Infinity;

        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, attractionRadius);
        foreach (Collider col in hitcolliders)
        {
            //if col != ourself
            if (col.transform.root != transform)
            {
                MergerV2 target = col.GetComponent<MergerV2>();
                
                //if target doesnt have merger script, go to next object;
                if (target != null)
                {
                    //if target mergeType is different, go to next object;
                    if (target.mergeType == mergeType)
                    {
                        //if seed or produce are different types, go to next object
                        //or if target is full, go to next
                        if (mergeType == MergeType.SEED)
                        {
                            if (GetComponent<SeedScript>().plantPrefab != col.GetComponent<SeedScript>().plantPrefab)
                                continue;
                            if (target.GetComponent<SeedScript>().NumberOfSeeds >= target.stackLimit)
                                continue;
                        }
                        else if (mergeType == MergeType.PRODUCE)
                        {
                            if (GetComponent<PlantProduce>().produceName != col.GetComponent<PlantProduce>().produceName)
                                continue;
                            if (target.GetComponent<PlantProduce>().ProduceAmount >= target.stackLimit)
                                continue;
                        }

                        float distance = Vector3.Distance(transform.position, col.transform.position);
                        //if target is in range and is closer than current closest
                        if (distance <= attractionRadius && distance <= closestDist)
                        {
                            closestDist = distance;
                            closestMerge = col.transform;
                        }                        
                    }
                }
            }
        }
        //yield return new WaitForSeconds(0.5f);
    }

    void Merge(Transform other)
    {
        if (!gameObject.activeSelf || !other.gameObject.activeSelf)
            return;


        //if other is being held
        if (other.GetComponent<Pickupable>().BeingHeld)
        {
            return;
        }        

        if (mergeType == MergeType.PRODUCE)
        {
            PlantProduce otherPlant = other.GetComponent<PlantProduce>();
            PlantProduce mePlant = GetComponent<PlantProduce>();
            //mePlant.ProduceAmount += otherPlant.ProduceAmount;

            int produceTotal = mePlant.ProduceAmount + otherPlant.ProduceAmount;
            //if stacks can fit into 1 stack
            if (produceTotal <= stackLimit)
            {
                mePlant.ProduceAmount = produceTotal;
                otherPlant.ProduceAmount = 0;
                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
            }
            //else fill one, and rest in other
            else
            {
                mePlant.ProduceAmount = stackLimit;
                otherPlant.ProduceAmount = produceTotal - stackLimit;
            }
        }
        else if (mergeType == MergeType.SEED)
        {
            SeedScript mePlant = GetComponent<SeedScript>();
            SeedScript otherPlant = other.GetComponent<SeedScript>();
            //mePlant.NumberOfSeeds += otherPlant.NumberOfSeeds;

            int produceTotal = mePlant.NumberOfSeeds + otherPlant.NumberOfSeeds;
            //if stacks can fit into 1 stack
            if (produceTotal <= stackLimit)
            {
                mePlant.NumberOfSeeds = produceTotal;
                otherPlant.NumberOfSeeds = 0;
                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
            }
            //else fill one, and rest in other
            else
            {
                mePlant.NumberOfSeeds = stackLimit;
                otherPlant.NumberOfSeeds = produceTotal - stackLimit;
            }
        }

        //Play merging particle effect
        if (mergeParticles != null)
        {
            GameObject particles = (GameObject)Instantiate(mergeParticles, transform.position, transform.rotation);
            foreach (ParticleSystem system in particles.GetComponentsInChildren<ParticleSystem>())
            {
                system.Play();
            }
            Destroy(particles, particleLifeTime);
        }

        //See if new merge target is around
        FindMergeTarget();
        
    }

}
