using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaffNo3 : NetworkBehaviour
{
    [FMODUnity.EventRef]
    public string pickUpSound = "event:/priority/pick up object magic";
    [FMODUnity.EventRef]
    public string dropSound = "event:/priority/dropping object magic";

    private Animator anim;

    //game objects you can pick up
    //add them here first

    [SyncVar]
    RaycastHit Hit;

    public GameObject StaffGrabber;
    GameObject pullBackPosition;
    [SyncVar]
    bool objectheld;

    float timeLeft;

    public GameObject ChosenObj;
	public GameObject StaffEmitter;



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

    public float stillLerpStrength = 0.3f;
    public float movingLerpStrength = 1f;


    private Vector3 targetPosition;
    private Vector3 followPosition;
    private Vector3 pastTargetPosition;
    private Vector3 pastFollowPosition;
    public float superSmoothLerpStrength = 20f;

    private Rigidbody heldRigidBody;

    Vector3 idealPos;

    float walkTimer;
    // Use this for initialization
    void Start()
    {
        walkTimer = 0.075f;
        anim = transform.GetChild(0).GetChild(2).GetComponent<Animator>();
        //initialize the gameobjects here
        throwforce = throwForceMin;
        StaffGrabber = transform.FindChild("FirstCamera").FindChild("Staff Grabber").gameObject;
        pullBackPosition = transform.FindChild("FirstCamera").FindChild("PullBackPosition").gameObject;
        objectheld = false;
        timeLeft = 0.02f;
        //   GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().enabled = false;
       //StaffEmitter = transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        walkTimer -= Time.deltaTime;

        if (walkTimer < 0)
            anim.SetBool("Walking", false);


        if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f ||
            Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
        {
            anim.SetBool("Walking", true);
            walkTimer = 0.075f;
        }



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
			if (Input.GetMouseButtonDown (0)) 
			{
				Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.0f));

				if (Physics.Raycast (ray, out Hit, GrabDistance)) 
				{
					ChosenObj = Hit.collider.gameObject;

					if ((Hit.collider.gameObject.GetComponent<Pickupable> () != null)) 
					{
						//check that another player isn't holding the object
						if (!ChosenObj.GetComponent<Pickupable> ().BeingHeld) 
						{
                            heldRigidBody = ChosenObj.GetComponent<Rigidbody>();
							//ChosenObj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
							//CmdAssignAuthority();
							objectheld = true;
                            anim.SetBool("Drop", false);
                            anim.SetTrigger ("Pickup");
							// StaffEmitter.SetActive(true);
							StaffEmitter.GetComponent<ParticleSystem> ().Play ();

							ChosenObj.GetComponent<Pickupable> ().BeingHeld = true;
							//Debug.Log(ChosenObj.GetComponent<Pickupable>().beingHeld);
							ChosenObj.GetComponent<Rigidbody> ().useGravity = false;
							carriedItemID = ChosenObj.GetComponent<NetworkIdentity> ().netId;

							//ChosenObj.GetComponent<Rigidbody>().isKinematic = true;

							CmdPickedUp (carriedItemID);

							//Play Sound
							FMODUnity.RuntimeManager.PlayOneShot (pickUpSound, ChosenObj.transform.position);
							Physics.IgnoreCollision (GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider> (), ChosenObj.GetComponent<Collider>());

						} 
						else 
						{
							ChosenObj = null;
                            heldRigidBody = null;
                        }

					} 
					else if (Hit.transform.GetComponent<Interactable> () != null) 
					{
						Hit.transform.GetComponent<Interactable> ().onInteract (GetComponent<NetworkIdentity> ().netId);
						ChosenObj = null;
                        heldRigidBody = null;

                    }
                    else
                    {
                        ChosenObj = null;
                        heldRigidBody = null;
                    }
					//ChosenObj = null;
				}
			}
		}        
        //plants get destroyed sometimes while being held
        else if (ChosenObj == null) 
		{
			objectheld = false;
            heldRigidBody = null;
            SeedNumber.text = "";
			SeedType.text = "";
            //anim.SetBool ("Drop") = true;
            Drop();
            //anim.ResetTrigger("Drop");
		} 
		else if (!ChosenObj.gameObject.activeInHierarchy) 
		{
			objectheld = false;
            heldRigidBody = null;
            SeedNumber.text = "";
			SeedType.text = "";
			//anim.SetTrigger ("Drop");
            Drop();
            //anim.ResetTrigger("Drop");
            ChosenObj = null;
		}
        else
        {                            
            //ChosenObj.transform.position = Vector3.Lerp(ChosenObj.transform.position, StaffGrabber.transform.position, 0.5f);

            /*float posRatio = throwforce / (throwForceMax - throwForceMin);
            Vector3 idealPos = Vector3.Lerp(StaffGrabber.transform.position, pullBackPosition.transform.position, posRatio);

            Debug.Log(Input.GetAxis("Vertical"));
            if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f ||
                Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
            {
                ChosenObj.GetComponent<Rigidbody>().MovePosition(idealPos);
                
            }
            else
            {
                ChosenObj.transform.position = Vector3.Lerp(ChosenObj.transform.position, idealPos, stillLerpStrength);
            }*/



            //ChosenObj.transform.position = Vector3.MoveTowards(ChosenObj.transform.position, idealPos, lerpStrength * Time.deltaTime);
            //ChosenObj.GetComponent<Rigidbody>().MovePosition(idealPos);


            ChosenObj.GetComponent<Rigidbody>().useGravity = false;
            ChosenObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ChosenObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            timeLeft -= Time.deltaTime;

            Quaternion grabbedRotation = StaffGrabber.transform.rotation;
            //ChosenObj.GetComponent<Rigidbody>().MoveRotation(grabbedRotation * Quaternion.Euler(objectXRotation, objectYRotation, 0));
             

            if (Input.GetMouseButtonDown(1))
            {
                anim.SetTrigger("Charge");
                StaffEmitter.GetComponent<ParticleSystem>().emissionRate = Random.Range(40, 60);
                StaffEmitter.GetComponent<ParticleSystem>().startSize = Random.Range(0.3f, 0.6f);
            }
            if (Input.GetMouseButtonUp(1))
            {
                anim.SetTrigger("Release");
                StaffEmitter.GetComponent<ParticleSystem>().Stop();
                StaffEmitter.GetComponent<ParticleSystem>().emissionRate = Random.Range(10, 20);
                StaffEmitter.GetComponent<ParticleSystem>().startSize = Random.Range(0.1f, 0.25f);
            }
                        
            if (Input.GetMouseButton(1))
            {
                throwforce += ((throwForceMax - throwForceMin) / throwMaxChargeTime) * Time.deltaTime;
                throwforce = Mathf.Clamp(throwforce, throwForceMin, throwForceMax);
            }
            //if object held , drops the object
            if (Input.GetMouseButtonDown(0))
            {
                Drop();
                anim.SetBool("Drop", true);
                CmdDropped();
                CmdNullChosen();
                throwforce = throwForceMin;
                //Play Sound
                FMODUnity.RuntimeManager.PlayOneShot(dropSound, ChosenObj.transform.position);
            }
            //if object held , throws the object
            if (Input.GetMouseButtonUp(1))
            {
				Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>(), false);
                CmdThrowed(throwforce);
                CmdNullChosen();
                throwforce = throwForceMin;
                //Play Sound
                FMODUnity.RuntimeManager.PlayOneShot(dropSound, ChosenObj.transform.position);
            }
            //Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>());


            //Catapult crates get set to inactive        
            //if (ChosenObj.activeSelf == false)
            //{
            //    ChosenObj = null;
            //    anim.SetTrigger("Drop");
            //}
        }

    }

    Vector3 dampingVelocity;
    void FixedUpdate()
    {
        if (ChosenObj != null)
        {
            if (heldRigidBody != null)
            {
                heldRigidBody.MovePosition(Vector3.SmoothDamp(heldRigidBody.position, idealPos, ref dampingVelocity, 0.05f, stillLerpStrength, Time.fixedDeltaTime));//Vector3.Lerp(heldRigidBody.position, idealPos, stillLerpStrength * Time.fixedDeltaTime));
                heldRigidBody.MoveRotation(Quaternion.Lerp(heldRigidBody.rotation, StaffGrabber.transform.rotation, stillLerpStrength * Time.fixedDeltaTime));
            }
            else
                Debug.Log("Trying to move chosen object but heldRigidBody == null");
        }
    }

    void LateUpdate()
    {
        float posRatio = throwforce / (throwForceMax - throwForceMin);
        idealPos = Vector3.Lerp(StaffGrabber.transform.position, pullBackPosition.transform.position, posRatio);
    }

    public void Drop()
    {
        anim.SetBool("Drop", true);
        StaffEmitter.GetComponent<ParticleSystem>().Stop();
        StaffEmitter.GetComponent<ParticleSystem>().emissionRate = Random.Range(10, 20);
        StaffEmitter.GetComponent<ParticleSystem>().startSize = Random.Range(0.1f, 0.25f);
		throwforce = 0;
		if (ChosenObj != null)
			Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>(), false);
		
       
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
    public void CmdDropped()
    {
		Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>(), false);
        if (ChosenObj.GetComponent<Rigidbody>() != null)
            ChosenObj.GetComponent<Rigidbody>().useGravity = true;
        carriedItemID = NetworkInstanceId.Invalid;
        if (ChosenObj.GetComponent<Pickupable>() != null)
                ChosenObj.GetComponent<Pickupable>().BeingHeld = false;
        //ChosenObj.GetComponent<Rigidbody>().isKinematic = false;
        objectheld = false;
        if (ChosenObj.GetComponent<NetworkIdentity>() != null)
            ChosenObj.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
    }

    [Command]
    void CmdThrowed(float _throwForce)
    {
		Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), ChosenObj.GetComponent<Collider>(), false);
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


    Vector3 SuperSmoothLerp ( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float time, float speed)
    {
        Vector3 f = pastPosition - pastTargetPosition + (targetPosition - pastTargetPosition) / (speed * time);
        return targetPosition - (targetPosition - pastTargetPosition) / (speed * time) + f * Mathf.Exp(-speed * time);
    }

}

