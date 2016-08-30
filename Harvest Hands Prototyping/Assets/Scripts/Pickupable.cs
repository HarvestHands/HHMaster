using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Pickupable : NetworkBehaviour
{
    public NetworkAnimator anim;
    public delegate void OnPickedUp();
    public OnPickedUp onPickedUp = delegate { };

    void Start()
    {
        anim = GetComponent<NetworkAnimator>();
    }

    public bool BeingHeld
    {
        get { return beingHeld; }
        set
        {
            if (value == beingHeld) return;
            if (value == true) onPickedUp();

            beingHeld = value;
        }
    }

    [SyncVar]
    private bool beingHeld;
    [SyncVar]
    public NetworkInstanceId parentNetId;
       
}
