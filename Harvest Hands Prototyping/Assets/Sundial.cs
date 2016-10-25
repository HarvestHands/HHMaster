using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sundial : MonoBehaviour
{
    DayNightController dayNightController;
    public float rotationOffset = 0;
    public RectTransform rectTransform;
    public Text dayCounter;

	// Use this for initialization
	void Start ()
    {
        dayNightController = FindObjectOfType<DayNightController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 rotation = new Vector3(0, dayNightController.currentTimeOfDay * 360f + rotationOffset, 0);
        rectTransform.localRotation = Quaternion.Euler(0.0f, 0, dayNightController.currentTimeOfDay * 360f + rotationOffset);
        //rectTransform.localRotation.eulerAngles.z.Set(rotation);
        //rectTransform.localRotation.z = dayNightController.currentTimeOfDay * 360f;

        dayCounter.text = "Day: " + dayNightController.ingameDay;
    }
}
