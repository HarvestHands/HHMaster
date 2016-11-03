using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CatapultButton2 : MonoBehaviour
{
    public StorageCatapult catapult;
                   
	// Use this for initialization
	void Start ()
    {
        GetComponent<Interactable>().onInteract += LaunchCatapult;
    }
	
    void LaunchCatapult(NetworkInstanceId playerId)
    {
        int i = catapult.expectedIncome;
        catapult.CmdEmptyCatapult();
        if (i != 0)
            catapult.farmbank.RpcSpawnPriceText(catapult.expectedIncome);
        catapult.CmdPlayAnimation();
    }
    
}
