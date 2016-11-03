using UnityEngine;
using System.Collections;

public class SameSpawne : MonoBehaviour
{
    public GameObject thingPrefab;
    public Transform spawnLocation;
    public KeyCode spawnKey = KeyCode.P;
    public int spawnAmount = 10;

	// Use this for initialization
	void Start ()
    {
	
	}
<<<<<<< HEAD
	/*
=======
	
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(spawnKey))
        {
            if (thingPrefab != null && spawnLocation != null)
            {
                for (int i = 0; i < spawnAmount; ++i)
                {
                    Instantiate(thingPrefab, spawnLocation.position, spawnLocation.rotation);
                }
            }
        }
<<<<<<< HEAD
	}*/
=======
	}
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e
}
