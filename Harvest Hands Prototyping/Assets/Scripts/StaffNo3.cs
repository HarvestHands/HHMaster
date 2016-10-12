using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaffNo3 : NetworkBehaviour
{


    public Animator anim;

    //game objects you can pick up
    //add them here first
    
    [SyncVar]
    RaycastHit Hit;
    
    GameObject StaffGrabber;
    GameObject pullBackPosition;
    [SyncVar]
    bool objectheld;

    float timeLeft;
    
    public GameObject ChosenObj;
    
    
    [SyncVar]
    public float GrabDistance = 3.0f;
    [SyncVar]
    public NetworkInstanceId carriedItemID = NetworkInstanceId.Invalid;

    public float throwforce = 100;
    public float throwForceMin = 100f;
    public float throwForceMax = 600f;
    public float throwMaxChargeTime = 2;

    [Command]
    void CmdPickedUp(NetworkInstanceId pickedUpItemID)
    {
        carriedItemID = pickedUpItemID;
    }
    
    Transform from;
    Transform to;
    float turnspeed = 200.0f;
    float objectYRotation;
    float objectXRotation;

    public Text SeedNumber;
    public Text SeedType;


    // Use this for initialization
    void Start()
    {
        //initialize the gameobjects here
        throwforce = throwForceMin;
        StaffGrabber = transform.FindChild("FirstCamera").FindChild("Staff Grabber").gameObject;
        pullBackPosition = transform.FindChild("FirstCamera").FindChild("PullBackPosition").gameObject;
        objectheld = false;
        timeLeft = 0.02f;
     //   GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().enabled = false;
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
                    //ChosenObj.GetComponent<Rigidbody>().isKinematic = true;
                    objectheld = true;
                }
            }
            else
            {

                if (ChosenObj == null)
                {
                    objectheld = false;
                }
                else
                {
                       ChosenObj.GetComponent<Rigidbody>().MovePosition(StaffGrabber.transform.position);
                    
                    if (carriedItemID == NetworkInstanceId.Invalid)
                    {
                        ChosenObj.GetComponent<Rigidbody>().useGravity = true;
                        ChosenObj = null;
                        objectheld = false;

                    }
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
                        //Debug.Log("SUCESS");
                        //    ChosenObj = Hit.collider.gameObject;
                        //Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
                        //check that another player isn't holding the object
                        if (!ChosenObj.GetComponent<Pickupable>().BeingHeld)
                        {
                            //ChosenObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                            //CmdAssignAuthority();
                            objectheld = true;


                            ChosenObj.GetComponent<Pickupable>().BeingHeld = true;
                            //Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
                            ChosenObj.GetComponent<Rigidbody>().useGravity = false;
                            carriedItemID = ChosenObj.GetComponent<NetworkIdentity>().netId;

                            //ChosenObj.GetComponent<Rigidbody>().isKinematic = true;

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
            SeedNumber.text = "";
            SeedType.text = "";
        }
        else
        {
            ChosenObj.transform.position = Vector3.Lerp(ChosenObj.transform.position, StaffGrabber.transform.position, 0.5f);


            // staffmove();
            //ChosenObj.GetComponent<Rigidbody>().MovePosition(StaffGrabber.transform.position);


            float posRatio = throwforce / (throwForceMax - throwForceMin);
            Vector3 idealPos = Vector3.Lerp(StaffGrabber.transform.position, pullBackPosition.transform.position, posRatio);

            //ChosenObj.GetComponent<Rigidbody>().MovePosition(idealPos);

            //float posRatio = throwforce / (throwForceMax - throwForceMin);
            //Vector3 
            //Vector3 pullbackDir = new Vector3(pullBackPosition.transform.position - StaffGrabber.transform.position);

            // Vector3 idealPos = new Vector3(StaffGrabber)

            ChosenObj.GetComponent<Rigidbody>().useGravity = false;
            ChosenObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ChosenObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            timeLeft -= Time.deltaTime;

            Quaternion grabbedRotation = StaffGrabber.transform.rotation;
            ChosenObj.GetComponent<Rigidbody>().MoveRotation(grabbedRotation * Quaternion.Euler(objectXRotation, objectYRotation, 0));







            if (Input.GetKeyDown(KeyCode.E))
            {

            }

            if (ChosenObj.tag == "Seed")
            {
                var ChosenSeed = ChosenObj.GetComponent<SeedScript>();

                SeedNumber.text = ChosenSeed.NumberOfSeeds.ToString();

                SeedType.text = ChosenObj.GetComponent<SeedScript>().plantPrefab.GetComponent<Plantscript>().plantName;
            }

            if (ChosenObj.tag != "Seed")
            {
                SeedNumber.text = "";
                SeedType.text = "";
            }


            if (Input.GetMouseButton(1))
            {
                throwforce += ((throwForceMax - throwForceMin) / throwMaxChargeTime) * Time.deltaTime;
                throwforce = Mathf.Clamp(throwforce, throwForceMin, throwForceMax);
            }
            //if object held , drops the object
            if (Input.GetMouseButtonDown(0))
            {
                CmdDropped();
                CmdNullChosen();
                throwforce = throwForceMin;
            }
            //if object held , throws the object
            if (Input.GetMouseButtonUp(1))
            {
                CmdThrowed(throwforce);
                CmdNullChosen();
                throwforce = throwForceMin;
            }
            RotateObject();


           //  Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>());
             //   Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag("Player").layer, ChosenObj.layer);



        //    Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>());



          

               Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>());

          //  Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag("Player").layer, ChosenObj.layer);
        }
    }
    [Command]
    void CmdTipBucket(NetworkInstanceId id)
    {
        var o = ClientScene.FindLocalObject(id);
        o.GetComponent<Pickupable>().anim.SetTrigger("play");
        //o.GetComponent<Water>().GetComponent<ParticleSystem>().CmdPlayParticles();
        //o.GetComponent<Water>().pouringParticleSystem.CmdPlayParticles();
        //o.GetComponent<Water>().RpcPlayParticles();

    }

    [Command]
    void CmdSwingScythe(NetworkInstanceId id)
    {
        var o = ClientScene.FindLocalObject(id);
        o.GetComponent<Pickupable>().anim.SetTrigger("play");
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
        if (ChosenObj != null)
        {
            ChosenObj.GetComponent<Pickupable>().BeingHeld = false;
            ChosenObj = null;
        }
        SeedNumber.text = "";
        SeedType.text = "";
        //Debug.Log("inside rpc");        
        carriedItemID = NetworkInstanceId.Invalid;
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
        ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        carriedItemID = NetworkInstanceId.Invalid;
        ChosenObj.GetComponent<Pickupable>().BeingHeld = false;
        //ChosenObj.GetComponent<Rigidbody>().isKinematic = false;
        objectheld = false;
        ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }

    [Command]
    void CmdThrowed(float _throwForce)
    {
        ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        ChosenObj.GetComponent<Rigidbody>().AddForce(StaffGrabber.transform.forward * _throwForce);
        carriedItemID = NetworkInstanceId.Invalid;
        ChosenObj.GetComponent<Pickupable>().BeingHeld = false;
        objectheld = false;
        ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }


    [Command]
    public void CmdAssignAuthority()
    {
        ChosenObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
    }

}

