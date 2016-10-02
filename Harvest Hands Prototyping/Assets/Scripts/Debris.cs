using UnityEngine;
using System.Collections.Generic;

public class Debris : MonoBehaviour
{
    public int requiredShredderTier = 1;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(this);
        //Debug.Log(Shredder.tier + " - shredder Tier");
        //Debug.Log("Debris List length b4 - " + Shredder.debrisList.Count);
        //
        Shredder.debrisList.Add(this.GetComponent<Debris>());
        //Debug.Log("Debris List length after- " + Shredder.debrisList.Count);
        //
        //Debug.Log("debrisReqTier = " + requiredShredderTier + " -- Shredder tier = " + Shredder.tier);
        //Debug.Log("debrisReqTier = " + requiredShredderTier + " -- Shredder tier = " + shredder.GetComponent<Shredder>().tier);


        Debug.Log("highestShredderTier = " + Shredder.highestTierShredder, gameObject);
        //if (shredder.GetComponent<Shredder>().tier >= requiredShredderTier)
        if (Shredder.highestTierShredder >= requiredShredderTier)
        {
            gameObject.AddComponent<Pickupable>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
