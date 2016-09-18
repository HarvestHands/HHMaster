using UnityEngine;
using System.Collections;

public class MergerV2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.tag == "Produce")
        {

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Produce").Length; i++)
        {
            // Debug.Log(GameObject.FindGameObjectsWithTag("ShopItemA").Length);


            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.transform.position, GameObject.FindGameObjectsWithTag("Produce")[i].transform.position) <= 1 && GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj != GameObject.FindGameObjectsWithTag("Produce")[i]) // > 2 && SpawnA == true)
            {

                Instantiate(Resources.Load("PlantProduceMed"), GameObject.FindGameObjectsWithTag("Produce")[i].transform.position, GameObject.FindGameObjectsWithTag("Produce")[i].transform.rotation);


                Destroy(GameObject.FindGameObjectsWithTag("Produce")[i]);
                Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj);

            }
        }
       }
	}
}
