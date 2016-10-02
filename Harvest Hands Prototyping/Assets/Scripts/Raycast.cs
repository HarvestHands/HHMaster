using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{

    private GameObject last;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
			if (hit.transform.GetComponent<pickupable2>() != null)
    		{
        	hit.transform.GetComponent<pickupable2>().hit = true;
        	//hit.transform.GetComponent<pickupable>().FresnelAmount += Time.deltaTime * hit.transform.GetComponent<pickupable>().FresnelIncrease;

    		}
        }



    }
}
