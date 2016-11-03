using UnityEngine;
using System.Collections;

public class SkyboxAnimationScript : MonoBehaviour {
    [SerializeField]
    AnimationCurve Atmosphere_Thickness;
    [SerializeField]
    AnimationCurve Exposure;

    //public Gradient colourGradiant;
    public float num;
    public float gross;
    // Use this for initialization
    void Start ()
    {
        //GetComponent<Renderer>().sharedMaterial = GetComponent<Renderer>().material;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateskybox(GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay);
    }


    public void updateskybox(float timeOfDay)
    {
        num = Atmosphere_Thickness.Evaluate(timeOfDay);
        gross = Exposure.Evaluate(timeOfDay);
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", num);
        RenderSettings.skybox.SetFloat("_Exposure", gross);
    }
}
