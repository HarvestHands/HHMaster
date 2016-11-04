using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CatapultButton2 : MonoBehaviour
{
    public StorageCatapult catapult;
    private float timer;
    public float upSpeed;
    public float downSpeed;
    private bool down;

    public Transform transDown;
    public Transform transUp;

    // Use this for initialization
    void Start()
    {

        GetComponent<Interactable>().onInteract += LaunchCatapult;
    }

    void Update()
    {
        if (down)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, transUp.rotation, upSpeed);

            else
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, transDown.rotation, downSpeed);
            Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    void LaunchCatapult(NetworkInstanceId playerId)
    {
        catapult.CmdEmptyCatapult();

        down = true;
        timer = 2;
    }

}
