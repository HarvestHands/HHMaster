using UnityEngine;
using System.Collections;

public class mouseLook: MonoBehaviour {

	public float lookSensitivity;
	public float xRotation;
	public float yRotation;
	public float currentxRotation;
	public float currentyRotation;
	public float yRotationV;
	public float xRotationV;
	public float lookSmoothDamp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		yRotation += Input.GetAxis ("Mouse X") * lookSensitivity;

		xRotation += Input.GetAxis ("Mouse Y") * lookSensitivity;

		xRotation = Mathf.Clamp (xRotation, -90, 90);

		currentxRotation = Mathf.SmoothDamp (currentxRotation, -xRotation, ref xRotationV, lookSmoothDamp);

		currentyRotation = Mathf.SmoothDamp (currentyRotation, yRotation, ref yRotationV, lookSmoothDamp);

		transform.rotation = Quaternion.Euler (currentxRotation, currentyRotation, 0);
	
	}
}
