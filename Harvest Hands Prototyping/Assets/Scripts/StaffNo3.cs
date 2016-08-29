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
    [SyncVar]
    bool objectheld;

    float timeLeft;
    
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
                        if (!ChosenObj.GetComponent<Pickupable>().beingHeld)
                        {
                            //ChosenObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                            //CmdAssignAuthority();
                            objectheld = true;


                            ChosenObj.GetComponent<Pickupable>().beingHeld = true;
                            //Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
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
            SeedNumber.text = "";
            SeedType.text = "";
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
            // ChosenObj.GetComponent<Animator>().SetBool("play", false);
            //  anim.SetBool("play", false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ChosenObj.tag == "Bucket")
                {
                    Debug.Log("Bucket");
                    Water water = ChosenObj.GetComponent<Water>();
                    if (water.waterlevel > 0)
                    {
                        water.waterlevel -= water.waterdrain;
                        //play particle && enable emmision

                        //play anim
                        CmdTipBucket(ChosenObj.GetComponent<Pickupable>().netId);
                        //spawn collider
                        GameObject WaterDrip = (GameObject)Instantiate(Resources.Load("PuddlePrefab"), ChosenObj.transform.position, ChosenObj.transform.rotation);
                    }

                }
                else if (ChosenObj.tag == "Scythe")
                {
                    Scythe scythe = ChosenObj.GetComponent<Scythe>();
                    Debug.Log("Scythe");
                    //play particle && enable emision

                    //play anim
                    CmdSwingScythe(ChosenObj.GetComponent<Pickupable>().netId);
                    //spawn collider
                    scythe.ActivateCutting();
                    scythe.Invoke("DeactiveCutting", 1.5f);


                }
            }

            if (ChosenObj.tag == "Seed")
            {
                //var SeedNumber = GameObject.Find("SeedNumber");
                //var NumberText = SeedNumber.GetComponent<Text>();

                var ChosenSeed = ChosenObj.GetComponent<SeedScript>();

                //NumberText.text = ChosenSeed.NumberOfSeeds.ToString();
                
                SeedNumber.text = ChosenSeed.NumberOfSeeds.ToString();

                //SeedNumber.text = "None";
                //SeedType.text = "None";


                //SeedType.text = ChosenObj.GetComponent<SeedScript>().plantPrefab.ToString();
                //SeedType.text = ChosenObj.GetComponent<SeedScript>().plantPrefab.plantName;
                SeedType.text = ChosenObj.GetComponent<SeedScript>().plantPrefab.GetComponent<Plantscript>().plantName;
            }

            if (ChosenObj.tag != "Seed")
            {
                SeedNumber.text = "";
                SeedType.text = "";
            }



            //if object held , drops the object
            if (Input.GetMouseButtonDown(0))
            {                
                CmdDropped();
                CmdNullChosen();
            }
            //if object held , throws the object
            if (Input.GetMouseButtonDown(1))
            {
                CmdThrowed();
                CmdNullChosen();
            }
            RotateObject();
        }
    }

    [Command]
    void CmdTipBucket(NetworkInstanceId id)
    {
        var o = ClientScene.FindLocalObject(id);
        o.GetComponent<Pickupable>().anim.SetTrigger("play");
        //o.GetComponent<Water>().GetComponent<ParticleSystem>().CmdPlayParticles();
        o.GetComponent<Water>().pouringParticleSystem.CmdPlayParticles();
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
            ChosenObj.GetComponent<Pickupable>().beingHeld = false;
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
        ChosenObj.GetComponent<Pickupable>().beingHeld = false;
        objectheld = false;
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
        ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }


    [Command]
    public void CmdAssignAuthority()
    {
        ChosenObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
    }

}

