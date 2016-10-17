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
        Debug.Log("Inside FindMergeTarget");
        if (mergeType == MergeType.NONE)
            return;
            //yield return new WaitForSeconds(0.5f);

        float closestDist = Mathf.Infinity;
        closestMerge = null;

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
                        if (mergeType == MergeType.SEED)
                        {
                            if (GetComponent<SeedScript>().plantPrefab != col.GetComponent<SeedScript>().plantPrefab)
                            continue;
                        }
                        else if (mergeType == MergeType.PRODUCE)
                        {
                            if (GetComponent<PlantProduce>().produceName != col.GetComponent<PlantProduce>().produceName)
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
            GetComponent<PlantProduce>().ProduceAmount += other.GetComponent<PlantProduce>().ProduceAmount;
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        else if (mergeType == MergeType.SEED)
        {
            GetComponent<SeedScript>().NumberOfSeeds += other.GetComponent<SeedScript>().NumberOfSeeds;
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
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
