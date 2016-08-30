using UnityEngine;
using System.Collections;

public class Mushroom : MonoBehaviour
{
    public int score = 3;


    [HideInInspector]
    public GameObject mushroomSpawner;
    
    void WasPickedUp()
    {
        Debug.Log("Mushroom Picked Up!", gameObject);
        if (mushroomSpawner != null)
        {            
            mushroomSpawner.GetComponent<MushroomSpawner>().canSpawn = true;
        }
    }

	// Use this for initialization
	void Start ()
    {
        GetComponent<Pickupable>().onPickedUp += WasPickedUp;
    }
	
}
