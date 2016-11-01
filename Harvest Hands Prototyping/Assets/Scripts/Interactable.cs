using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour
{
    //Takes ID of player who is interacting with object
    public delegate void OnInteract(NetworkInstanceId playerID);
    public OnInteract onInteract = delegate { };

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
