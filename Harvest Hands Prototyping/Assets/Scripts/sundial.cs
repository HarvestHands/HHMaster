using UnityEngine;
using System.Collections;

public class sundial : MonoBehaviour {

	public GameObject GameM;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	//	GameM.GetComponent<DayNightController> ().currentTimeOfDay;

		gameObject.transform.Rotate (Vector3.back * (GameM.GetComponent<DayNightController> ().currentTimeOfDay*10));

	}
}
