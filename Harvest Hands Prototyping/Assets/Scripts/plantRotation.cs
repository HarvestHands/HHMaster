using UnityEngine;
using System.Collections;

public class plantRotation : MonoBehaviour {

	public GameObject plant;

	public Vector3 sw;

	// Use this for initialization
	void Start () {
	
		plant = gameObject;

		sw = new Vector3 (0, 0, 0);

		sw.y = Random.Range (0, 360);

		//plant.transform.rotation.y = sw.y;

	}
	// Update is called once per frame
	void Update () {
	
	}
}
