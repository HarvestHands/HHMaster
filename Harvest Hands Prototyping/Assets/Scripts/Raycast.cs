using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{

    private GameObject last;
	public float rayCastDistance = 3f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));

		if (Physics.Raycast (ray, out hit, rayCastDistance)) 
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
			if (hit.transform.GetComponent<pickupable2>() != null)
    		{
        	hit.transform.GetComponent<pickupable2>().hit = true;
        	//hit.transform.GetComponent<pickupable>().FresnelAmount += Time.deltaTime * hit.transform.GetComponent<pickupable>().FresnelIncrease;

    		}
        }



    }
}
