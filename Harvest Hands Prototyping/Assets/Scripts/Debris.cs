﻿using UnityEngine;
using System.Collections.Generic;

public class Debris : MonoBehaviour
{
    public int requiredShredderTier = 1;
    Vector3 startpos;

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


        startpos = new Vector3(355.33f, 231.85f, 82.48006f);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.75)
        {
            this.gameObject.GetComponent<Rigidbody>().MovePosition(startpos);
        }

	}
}
