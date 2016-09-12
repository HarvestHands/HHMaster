using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class waterBox : MonoBehaviour {

	public GameObject underWaterUI;
	public float drownTimer = 20f;
	public Slider oxySlider;
	public GameObject spawnPoint;

	void Start(){

		oxySlider.maxValue = 20f;

	}

	void OnTriggerStay(Collider plr){

		if (plr.gameObject.tag == "Player") {

			underWaterUI.GetComponent<Canvas> ().enabled = true;
		} 

		drownTimer -= Time.deltaTime;
		if (drownTimer < 0) {

			plr.transform.position = spawnPoint.transform.position;
			Debug.Log ("Dead af lol");

		}

		oxySlider.value = drownTimer;
	}

	void OnTriggerExit(Collider plr){

		underWaterUI.GetComponent<Canvas> ().enabled = false;

		drownTimer = 20f;
	}
}
