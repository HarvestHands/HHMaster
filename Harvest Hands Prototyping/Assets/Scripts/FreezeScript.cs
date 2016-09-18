using UnityEngine;
using System.Collections;

public class FreezeScript : MonoBehaviour {

    bool unlockobj;


	// Use this for initialization
	void Start () {

        unlockobj = false;

	}
	
	// Update is called once per frame
	void Update () {





        if (Input.GetKeyDown(KeyCode.Y) && GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().isKinematic == false)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().isKinematic = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;      
            GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Pickupable>().enabled = false;      
        }



        if (Input.GetKeyDown(KeyCode.U) && GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().isKinematic == true)
        {
           GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().isKinematic = false;
           GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
           GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Pickupable>().enabled = true;
        }



	}
}
