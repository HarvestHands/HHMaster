using UnityEngine;
using System.Collections;

public class Bed : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GetComponent<Interactable>().onInteract += Activate;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Activate()
    {
        GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay = 0.749f;
    }
}
