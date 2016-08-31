using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour
{
    public int requiredShredderTier = 1;
    GameObject shredder;

	// Use this for initialization
	void Start ()
    {
        Shredder.debrisList.Add(this);

        if (shredder.GetComponent<Shredder>().tier >= requiredShredderTier)
            gameObject.AddComponent<Pickupable>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
