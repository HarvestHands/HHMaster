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
<<<<<<< HEAD
        int i = catapult.expectedIncome;
        catapult.CmdEmptyCatapult();
        catapult.farmbank.RpcSpawnPriceText(catapult.expectedIncome);
=======
        catapult.CmdEmptyCatapult();
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e
    }
    
}
