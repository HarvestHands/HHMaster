using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class Water : NetworkBehaviour
{
            
    public GameObject BucketWater;
    public ParticleSystemScript pouringParticleSystem;
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
                    }
                }
            }
        }

		if (col.gameObject.tag == "Water")
		{
			waterlevel = waterfill;
			BucketWater.SetActive(true);
			AdjustWaterLevel();
			GameObject refillSplash = (GameObject)Instantiate(refillParticles, transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
			Destroy(refillSplash, 2);
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
        }
        else
        {
            drippingParticleSystem.PlayParticles();
        }

        if (waterlevel > 0 && waterlevel <= 1)
        {
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.2f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            //  BucketWater.transform.position.Set(BucketWater.transform.position.x, 0.3f, BucketWater.transform.position.z);
        }
        if (waterlevel > 1 && waterlevel <= 2)
        {            
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.5f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            // BucketWater.transform.position.Set(BucketWater.transform.position.x,0.5f,BucketWater.transform.position.z);
        }
        if (waterlevel > 2 && waterlevel <= 3)
        {
            Vector3 tmpPos = BucketWater.transform.localPosition; // Store all Vector3
            tmpPos.y = 0.7f; // example assign individual fox Y axe
            BucketWater.transform.localPosition = tmpPos; // Assign back all Vector3
            //BucketWater.transform.position.Set(BucketWater.transform.position.x, 0.7f, BucketWater.transform.position.z);
        }
    }
    

}
