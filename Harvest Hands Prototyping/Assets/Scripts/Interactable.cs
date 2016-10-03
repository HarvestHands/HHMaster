using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public delegate void OnInteract();
    public OnInteract onInteract = delegate { };

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
