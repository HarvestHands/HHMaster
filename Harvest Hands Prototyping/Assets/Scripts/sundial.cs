using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sundial : MonoBehaviour {

	public GameObject GameM;

	public Text time;

	// Use this for initialization
	void Start () {

		GameM = GameObject.FindGameObjectWithTag ("GameManager");
	
	}
	
	// Update is called once per frame
	void Update () {

	//	GameM.GetComponent<DayNightController> ().currentTimeOfDay;

		time.text = (GameM.GetComponent<DayNightController> ().currentTimeOfDay).ToString();

	}
}
