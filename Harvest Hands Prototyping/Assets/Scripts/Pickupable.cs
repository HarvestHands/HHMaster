using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickupable : NetworkBehaviour
{
    public NetworkAnimator anim;

    void Start()
    {
        anim = GetComponent<NetworkAnimator>();
    }



    [SyncVar]
    public bool beingHeld;
    [SyncVar]
    public NetworkInstanceId parentNetId;
       
}
