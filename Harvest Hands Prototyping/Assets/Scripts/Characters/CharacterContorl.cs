using UnityEngine;
using System.Collections;

public class CharacterContorl : MonoBehaviour
{
    private CharacterController controller;

    Vector3 move = Vector3.zero;

    public float turnSpeed = 90f;

    public float walkSpeed = 1f;
    public float runSpeed = 5f;
    public float jumpSpeed = 5f;
    public float acceleration = 1f;
    public float currentSpeed = 0f;

    public bool jump = false;
    public bool isRunning = false;
    private Vector3 gravity = Vector3.zero;

    public mouseLook mouselook;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Mouse Look
        transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f);

        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        jump = Input.GetKey(KeyCode.Space);

        currentSpeed += acceleration * Input.GetAxis("Vertical");
        if (isRunning)
            currentSpeed = Mathf.Clamp(currentSpeed, -runSpeed, runSpeed);
        else
            currentSpeed = Mathf.Clamp(currentSpeed, -walkSpeed, walkSpeed);

        if (Input.GetAxis("Vertical") == 0)
        {
            if (currentSpeed > acceleration) currentSpeed -= acceleration;// * Time.deltaTime;
            if (currentSpeed < -acceleration) currentSpeed += acceleration;// * Time.deltaTime;
        }

        move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move.Normalize();
        move = transform.TransformDirection(move);
        move *= currentSpeed;

        //if (isRunning)
        //    move *= runSpeed;
        //else
        //    move *= walkSpeed;

        if (!controller.isGrounded)
        {
            gravity += Physics.gravity * Time.deltaTime;
        }
        else
        {
            gravity = Vector3.zero;
            if (jump)
            {
                gravity.y = jumpSpeed;
                jump = false;
            }
        }


        move += gravity;

        controller.Move(move * Time.deltaTime);
        Debug.Log(move);
	}
}
