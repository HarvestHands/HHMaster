﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class waterBox : MonoBehaviour {

	public GameObject underWaterUI;
	public float drownTimer = 10f;
	public Slider oxySlider;
	public GameObject spawnPoint;
	//public GameObject player;

	void Start(){

		oxySlider.maxValue = drownTimer;

	}

	void OnTriggerStay(Collider plr){

		if (plr.gameObject.tag == "Player") {

			underWaterUI.GetComponent<Canvas> ().enabled = true;
		
			
			//GameObject.GetComponent<FirstPersonController> ().walkSpeed;
			//GameObject.GetComponent<FirstPersonController> ().runSpeed;

			drownTimer -= Time.deltaTime;
			if (drownTimer < 0) {

				plr.transform.position = spawnPoint.transform.position;
				Debug.Log ("Dead af lol");
              

                GameObject.Find("GameManager").GetComponent<DayNightController>().playerdeathcount += 1;
                GameObject.Find("GameManager").GetComponent<DayNightController>().PlayerDeathPenalty();                
                if (plr.GetComponent<StaffNo3>().ChosenObj != null)
                    plr.GetComponent<StaffNo3>().CmdDropped();

                plr.GetComponent<DeathFade>().CmdShowDrownText();
                plr.GetComponent<DeathFade>().RpcSetShowDeathPenaltyImage(true);
                plr.GetComponent<DeathFade>().RpcPlayDeathSound();
                plr.GetComponent<PlayerInventory>().RpcApplyDeathPenalty();
            }

		}
		oxySlider.value = drownTimer;
	}

	void OnTriggerExit(Collider plr){

		if (plr.gameObject.tag == "Player") {
			underWaterUI.GetComponent<Canvas> ().enabled = false;

			drownTimer = oxySlider.maxValue;
		}
	}
}
