using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkBypass : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Invoke("StartHost", 0.05f);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }
}
