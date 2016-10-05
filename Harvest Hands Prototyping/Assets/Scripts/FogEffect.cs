using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogEffect : MonoBehaviour
{
    [SerializeField]
    AnimationCurve densityCurve;
    public Gradient colourGradiant;
    public float num;
	// Use this for initialization
	void Start () {
        num = Mathf.Lerp(0, 1, Mathf.InverseLerp(0.002f, 0.05f, 0.03f));

    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateFog(GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay);
        }

    public void UpdateFog(float timeOfDay)
    {
        RenderSettings.fogColor = colourGradiant.Evaluate(timeOfDay);
        //float alphaTemp = colourGradiant.Evaluate(timeOfDay).a;
        //Debug.Log(alphaTemp);
        num = densityCurve.Evaluate(timeOfDay);//Mathf.Lerp(0.002f, 0.05f, alphaTemp);
        RenderSettings.fogDensity = num;
        
        
        }
}
