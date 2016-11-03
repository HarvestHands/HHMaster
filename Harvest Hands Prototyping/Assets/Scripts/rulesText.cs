using UnityEngine;
using System.Collections;

public class rulesText : MonoBehaviour {

	public GameObject rulesCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider can){

		if (can.tag == "Player") {

			Destroy (rulesCanvas);

		}
	}
}
