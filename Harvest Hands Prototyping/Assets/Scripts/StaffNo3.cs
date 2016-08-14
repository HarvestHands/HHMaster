using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class StaffNo3 : NetworkBehaviour
{




    //game objects you can pick up
    //add them here first




    [SyncVar]
    RaycastHit Hit;

    //remove syncvar
    //[SyncVar] 
    GameObject StaffGrabber;


    [SyncVar]
    bool objectheld;


    float timeLeft;


    // [SyncVar]
    public GameObject ChosenObj;





    [SyncVar]
    public float throwforce = 500f;

    [SyncVar]
    public float GrabDistance = 3.0f;

    [SyncVar]
    public NetworkInstanceId carriedItemID = NetworkInstanceId.Invalid;




    [Command]
    void CmdPickedUp(NetworkInstanceId pickedUpItemID)
    {
        carriedItemID = pickedUpItemID;
    }

    //   NetworkConnection con1 = NetworkPlayer




    //  [Client]
    //  void staffmove()
    //  {
    //      StaffGrabber = transform.FindChild("FirstCamera").FindChild("Staff Grabber").gameObject;

    //  }


    Transform from;
    Transform to;
    float turnspeed = 200.0f;
    float objectYRotation;
    float objectXRotation;






    // Use this for initialization
    void Start()
    {
        //initialize the gameobjects here

        StaffGrabber = transform.FindChild("FirstCamera").FindChild("Staff Grabber").gameObject;
        objectheld = false;
        timeLeft = 0.02f;




    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            if (!objectheld)
            {
                if (carriedItemID != NetworkInstanceId.Invalid)
                {
                    GameObject heldItem = ClientScene.FindLocalObject(carriedItemID);
                    ChosenObj = heldItem;


                    ChosenObj.GetComponent<Rigidbody>().useGravity = false;
                    objectheld = true;
                }
            }
            else
            {

                if (ChosenObj == null)
                {
                    objectheld = false;
                }
                ChosenObj.GetComponent<Rigidbody>().MovePosition(StaffGrabber.transform.position);
                
                if (carriedItemID == NetworkInstanceId.Invalid)
                {
                    ChosenObj.GetComponent<Rigidbody>().useGravity = true;
                    ChosenObj = null;
                    objectheld = false;

                }
            }
            return;
        }       

        if (objectheld == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                if (Physics.Raycast(ray, out Hit, GrabDistance))
                {
                    ChosenObj = Hit.collider.gameObject;

                    if ((Hit.collider.gameObject.GetComponent<Pickupable>() != null))
                    {
                        Debug.Log("SUCESS");
                        //    ChosenObj = Hit.collider.gameObject;
                        Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
                        //check that another player isn't holding the object
                        if (!ChosenObj.GetComponent<Pickupable>().beingHeld)
                        {
                            objectheld = true;
                            
                            ChosenObj.GetComponent<Pickupable>().beingHeld = true;
                            Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
                            ChosenObj.GetComponent<Rigidbody>().useGravity = false;
                            carriedItemID = ChosenObj.GetComponent<NetworkIdentity>().netId;

                            CmdPickedUp(carriedItemID);

                        }
                        else
                        {
                            ChosenObj = null;
                        }

                    }
                }
            }
        }
        //plants get destroyed sometimes while being held
        else if (ChosenObj == null)
        {
            objectheld = false;
        }
        else
        {
            // staffmove();
            ChosenObj.GetComponent<Rigidbody>().MovePosition(StaffGrabber.transform.position);
            
            ChosenObj.GetComponent<Rigidbody>().useGravity = false;
            ChosenObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ChosenObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            timeLeft -= Time.deltaTime;


            Quaternion grabbedRotation = StaffGrabber.transform.rotation;
            ChosenObj.GetComponent<Rigidbody>().MoveRotation(grabbedRotation * Quaternion.Euler(objectXRotation, objectYRotation, 0));


            //if bucket held , throws the bucket
            if (Input.GetMouseButtonDown(0))
            {
                CmdDropped();
                Debug.Log("calling rpc");
                CmdNullChosen();

                Debug.Log("rpc done");
            }
            

            if (Input.GetMouseButtonDown(1))
            {
                CmdThrowed();
                Debug.Log("calling rpc");
                RpcNullChosen();
                Debug.Log("rpc done");
                ChosenObj = null;
            }
            RotateObject();
        }
    }


    void OnCollisionEnter(Collision coll)
    {   //if bucket colides with something push it back to the grab location
        //ChosenObj.transform.position = Vector3.MoveTowards(transform.position, StaffGrabber.transform.position, 1);
    }

    void RotateObject()
    {
        var rot = Input.GetAxis("Mouse ScrollWheel");
        //up
        if (rot > 0f)
        {
            objectYRotation += turnspeed * Time.deltaTime;
            objectYRotation += turnspeed * Time.deltaTime;
            if (objectYRotation >= 360) objectYRotation -= 360;
        }

     

        //down
        if (rot < 0f)
        {
            objectXRotation += turnspeed * Time.deltaTime;
            if (objectXRotation >= 360) objectXRotation -= 360;

        }
    }

    [ClientRpc]
    void RpcNullChosen()
    {
        Debug.Log("inside rpc");
        ChosenObj.GetComponent<Pickupable>().beingHeld = false;
        ChosenObj = null;
        objectheld = false;

    }

    [Command]
    void CmdNullChosen()
    {
        RpcNullChosen();
    }

    [Command]
    void CmdDropped()
    {
        //ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        objectheld = false;
        ChosenObj.GetComponent<Pickupable>().beingHeld = false;
        Debug.Log("Post-drop:" + ChosenObj.GetComponent<Pickupable>().beingHeld);
        carriedItemID = NetworkInstanceId.Invalid;
        ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }




    [Command]
    void CmdThrowed()
    {
        ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        ChosenObj.GetComponent<Rigidbody>().AddForce(transform.forward * throwforce);
        carriedItemID = NetworkInstanceId.Invalid;
        ChosenObj.GetComponent<Pickupable>().beingHeld = false;
        objectheld = false;
        Debug.Log("Post-throw:" + ChosenObj.GetComponent<Pickupable>().beingHeld);
        ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }

}

