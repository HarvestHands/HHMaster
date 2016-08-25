using UnityEngine;
using System.Collections;


public class RandomEvents : MonoBehaviour {

    float eventchance;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {



        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.50f && GameObject.FindGameObjectWithTag("GameManage").GetComponent<DayNightController>().currentTimeOfDay < 0.5001f)
        {
            

           eventchance = Mathf.RoundToInt(Random.Range(0f, 3f));

           Debug.Log(eventchance);

            if(eventchance == 2f)
            {
                Triggerevent1();
            }
            

        }


	
	}


    void Triggerevent1()
    {

        Debug.Log("EVENT!");
    
    }

}
