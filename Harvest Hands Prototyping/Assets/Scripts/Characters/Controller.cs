using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public float walkAcceleration = 3;
	public GameObject cameraObject;
	public Rigidbody rb;
	public float maxWalkSpeed = 10;
	public Vector2 horizontalMovement;
	public float jumpVelocity = 2000;
	public bool isGrounded = true;
	public float maxSlope = 60;
	public float sprintModifier = 1.4f;
	public bool drainStamina;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void Update () {

		//maxWalkSpeed = ((gameObject.GetComponent<Stats> ().Speed) * 2.3f);
		//walkAcceleration = (gameObject.GetComponent<Stats> ().Speed)+20;

		transform.rotation = Quaternion.Euler (0, (cameraObject.GetComponent<mouseLook> ().currentyRotation), 0);
		rb.AddRelativeForce (Input.GetAxis ("Horizontal") * walkAcceleration, 0, Input.GetAxis ("Vertical") * walkAcceleration);
		horizontalMovement = new Vector2 (rb.velocity.x, rb.velocity.z);

		if (horizontalMovement.magnitude > maxWalkSpeed) {
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxWalkSpeed;
		}
			
		rb.velocity = new Vector3 (horizontalMovement.x, rb.velocity.y, horizontalMovement.y);

		if (Input.GetButtonDown ("Jump") && isGrounded)
			rb.AddForce (0, jumpVelocity, 0);

		if (Input.GetButtonDown ("Sprint") && isGrounded)
			maxWalkSpeed = maxWalkSpeed * 2;
			horizontalMovement = horizontalMovement * sprintModifier;
			drainStamina = true;	
	}
		

	void GroundCheck()
	{
		RaycastHit hit;
		float distance = 1f;
		Vector3 dir = new Vector3(0, -1);

		if(Physics.Raycast(transform.position, dir, out hit, distance))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}
		
}
