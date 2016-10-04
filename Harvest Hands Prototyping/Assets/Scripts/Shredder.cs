using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Shredder : NetworkBehaviour
{
    public int tier = 1;
    public static int highestTierShredder = 1;
    public static List<Debris> debrisList = new List<Debris>();

    public ParticleSystemScript shreddedParticles;
    
	// Use this for initialization
	void Start ()
    {
        if (tier > Shredder.highestTierShredder)
        {
            Shredder.highestTierShredder = tier;
            UpdateDebris();
        }        
	}
	
	// Update is called once per frame
	void Update ()
    {
       // if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("oldtier " + tier);
            CmdUpgradeShredder(++tier);
        }
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

            //shreddedParticles.PlayParticles();
            shreddedParticles.CmdPlayParticles();
            shreddedParticles.StopLooping();
            //Destroy(col.gameObject);

            col.gameObject.SetActive(false);
        }
    }
    
    public void UpdateDebris()
    {
        
        Debug.Log(debrisList.Count + " debrisListCount");
        foreach (Debris debris in debrisList)
        {
            if (debris == null)
            {
                debrisList.Remove(debris);
                //debrisList./
                continue;
            }
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

    [Command]
    public void CmdUpgradeShredder(int newTier)
    {
        Debug.Log("Inside UpgradeShredder, new tier = " + newTier);
        RpcUpgradeShredder(newTier);
        tier = newTier;
        if (newTier > Shredder.highestTierShredder)
        {
            Shredder.highestTierShredder = tier;
            UpdateDebris();
        }
    }

    [ClientRpc]
    public void RpcUpgradeShredder(int newTier)
    {
        tier = newTier;
        if (newTier > Shredder.highestTierShredder)
        {
            Shredder.highestTierShredder = tier;
            UpdateDebris();
        }
    }
    
}
