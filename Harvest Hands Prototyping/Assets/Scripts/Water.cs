using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class Water : NetworkBehaviour
{
            
    public GameObject BucketWater;
    //public ParticleSystemScript pouringParticleSystem;
    public ParticleSystemScript drippingParticleSystem;
    public float particlePlayDuration;

    [Tooltip("Current water level")]
    [SyncVar]
    public float waterlevel = 0.0f;

    [Tooltip("How much is used to water a plant")]
    [SyncVar]
    public float waterdrain = 0.5f;

    [Tooltip("How much water the bucket can hold")]
    [SyncVar]
    public float waterfill = 3.0f;

    public GameObject refillParticles;
    public GameObject wateredParticles;

    [FMODUnity.EventRef]
    public string emptyBucketSound = "event:/Done/Empty Bucket";
    [FMODUnity.EventRef]
    public string refillBucketSound = "event:/Done/Watering Plant";

    // Use this for initialization
    void Start ()
    {
        if (waterlevel <= 0)
            drippingParticleSystem.StopParticles();
    }

   
	
	// Update is called once per frame
	void Update ()
    {

    }

   /* void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Water")
        {
            waterlevel = waterfill;
            BucketWater.SetActive(true);
            AdjustWaterLevel();
            GameObject refillSplash = (GameObject)Instantiate(refillParticles, transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
            Destroy(refillSplash, 2);
        }

        if (coll.gameObject.tag == "Plant")
        {
            Debug.Log("Watered by collision");
        }  
    }*/

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Plant")
        {
            //Debug.Log("Watered");
    
            if (waterlevel > 0)
            {                
                Plantscript plant = col.gameObject.GetComponent<Plantscript>();
                if (plant.isAlive)
                {
                    if (!plant.isWatered)
                    {
                        plant.isWatered = true;
                        waterlevel -= waterdrain;
                        AdjustWaterLevel();        
                        if (wateredParticles != null)
                        {
                            GameObject waterSplash = (GameObject)Instantiate(wateredParticles, transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
                            Destroy(waterSplash, plant.particlePlayDuration);
                        }
                        //Play Sound
                        FMODUnity.RuntimeManager.PlayOneShot(plant.wateredSound, col.transform.position);
                    }
                }
            }
        }

		if (col.gameObject.tag == "Water")
		{
			waterlevel = waterfill;
			BucketWater.SetActive(true);
			AdjustWaterLevel();
            if (refillParticles != null)
            {
                GameObject refillSplash = (GameObject)Instantiate(refillParticles, transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
                Destroy(refillSplash, 2);
            }
            //Play Sound
            FMODUnity.RuntimeManager.PlayOneShot(refillBucketSound, col.transform.position);
        }

		if (col.gameObject.tag == "Plant")
		{
			Debug.Log("Watered by collision");
		}  
    }

    void AdjustWaterLevel()
    {
        if (waterlevel <= 0)
        {
            drippingParticleSystem.StopParticles();
            BucketWater.SetActive(false);
            //Play Sound
            FMODUnity.RuntimeManager.PlayOneShot(emptyBucketSound, transform.position);
        }
        else
        {
            drippingParticleSystem.PlayParticles();
        }

        if (waterlevel > 0 && waterlevel <= 1)
        {
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.1f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            //  BucketWater.transform.position.Set(BucketWater.transform.position.x, 0.3f, BucketWater.transform.position.z);
        }
        if (waterlevel > 1 && waterlevel <= 2)
        {            
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.3f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            // BucketWater.transform.position.Set(BucketWater.transform.position.x,0.5f,BucketWater.transform.position.z);
        }
        if (waterlevel > 2 && waterlevel <= 3)
        {
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.5f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            //BucketWater.transform.position.Set(BucketWater.transform.position.x, 0.7f, BucketWater.transform.position.z);
        }
    }
    

}
