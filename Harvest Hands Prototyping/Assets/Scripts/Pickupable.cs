using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickupable : NetworkBehaviour
{
    [SyncVar]
    public bool beingHeld;
    [SyncVar]
    public NetworkInstanceId parentNetId;
       
}
