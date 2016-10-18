using UnityEngine;
using System.Collections;

public class MushRoomDed : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //destroys mushrooms after certain time in day
        if (GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.75 && GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay <= 0.76)
        {
            Destroy(gameObject);

        }
    }
}