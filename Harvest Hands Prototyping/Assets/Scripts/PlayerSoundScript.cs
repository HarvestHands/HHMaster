using UnityEngine;
using System.Collections;

public class PlayerSoundScript : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string dropSound = "event:/priority/dropping object magic";

    FMOD.Studio.EventInstance playerState;

    [FMODUnity.EventRef]
    public string market;
    FMOD.Studio.EventInstance marketSound;

    public bool inMarket = false;

    public Rigidbody rigidBody;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
            Debug.Log("Rigidbody == null");
        playerState = FMODUnity.RuntimeManager.CreateInstance(dropSound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerState, transform, rigidBody);
        playerState.start();
        Invoke("SpawnIntoWorld", 5f);
        Debug.Log("Calling start() pm playerState");
	}

    void OnDestroy()
    {
        playerState.release();
    }

    void SpawnIntoWorld()
    {
        Debug.Log("Calling SpawnIntoWorld");
        playerState.start();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //wtf was this for?
        //playerState.setParameterValue("health", 1);
	}
}
