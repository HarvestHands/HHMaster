using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkBypass : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        NetworkManager.singleton.StartHost();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
