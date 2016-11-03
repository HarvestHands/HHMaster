using UnityEngine;
using System.Collections.Generic;

public class Debris : MonoBehaviour
{
    public int requiredShredderTier = 1;
    Vector3 startpos;
    Vector3 SavedPos;
    ////////
    Quaternion SavedRot;


    // Use this for initialization
    void Start()
    {
       
        Shredder.debrisList.Add(this.GetComponent<Debris>());
       
      
        //if (shredder.GetComponent<Shredder>().tier >= requiredShredderTier)
        if (Shredder.highestTierShredder >= requiredShredderTier)
        {
            gameObject.AddComponent<Pickupable>();
        }


        startpos = new Vector3(355.33f, 231.85f, 82.48006f);
        SavedPos = new Vector3(0, 0, 0);


        

    }
	
	// Update is called once per frame
	void Update ()
    {
     

        if (GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.75 && GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay <= 0.76)
        {
            this.gameObject.GetComponent<Rigidbody>().MovePosition(SavedPos);

            /////////////////////////////////////////////////////////////////
            this.gameObject.GetComponent<Rigidbody>().MoveRotation(SavedRot);
        }



        if (GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.26 && GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay <= 0.27)
        {
            SavedPos = new Vector3(0, 0, 0);
            SavedPos += this.gameObject.GetComponent<Rigidbody>().position;
            //////////////////////////////////////////////////////////////
            SavedRot = new Quaternion();
            SavedRot = this.gameObject.GetComponent<Rigidbody>().rotation;
        }


	}
}
