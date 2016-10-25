using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bed : NetworkBehaviour
{
    public float setTimeTo = 0.78f;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Interactable>().onInteract += CmdActivate;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    [Command]
    public void CmdActivate()
    {
        GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay = setTimeTo;
        //TriggerFadeIn();
    }
    
    public void TriggerFadeIn()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            DeathFade fade = player.GetComponent<DeathFade>();
            fade.CmdBedFadeIn();
        }
    }
}
