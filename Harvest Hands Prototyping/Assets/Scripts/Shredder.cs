using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shredder : MonoBehaviour
{
    
    public int tier = 1;
    public ParticleSystemScript shreddedParticles;

    public static List<Debris> debrisList;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    //  ///////////////////////////////// TO DO TEST DEBRIS AND SHREDDER UPGRADING

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Pickupable>() != null)
        {
            if (GetComponent<Debris>() != null)
            {
                if (GetComponent<Debris>().requiredShredderTier > tier)
                { 
                    return;
                }
            }

            shreddedParticles.PlayParticles();
            shreddedParticles.StopLooping();
            Destroy(col.gameObject);
        }
    }

    public void UpdateDebris()
    {
        foreach (Debris debris in debrisList)
        {
            if (tier >= debris.requiredShredderTier)
            {
                if (debris.GetComponent<Pickupable>() != null)
                {
                    Debug.Log(name + " tried to add pickupable to " + debris.name + " - (debris) already has pickupable");
                    continue;
                }
                debris.gameObject.AddComponent<Pickupable>();
            }
        }
    }
}
