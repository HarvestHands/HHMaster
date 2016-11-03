using UnityEngine;
using System.Collections;

public class ColorSingleton : MonoBehaviour {

    public static ColorSingleton instance = null;
    public Color PickUp;
    public Color Interact;
    public Color Buy;

	// Use this for initialization
	void Start ()
    {
	    if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
	}

    
	
	// Update is called once per frame
	void Update () {
	
	}
}
