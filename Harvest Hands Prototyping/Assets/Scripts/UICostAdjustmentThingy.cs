using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICostAdjustmentThingy : MonoBehaviour
{
    private RectTransform rectTrans;
    public float yDelta = 1f;
    public float alphaFadeDuration = 2;
     
	// Use this for initialization
	void Awake ()
    {
        rectTrans = GetComponent<RectTransform>();
        GetComponent<Text>().CrossFadeAlpha(0, alphaFadeDuration, true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        rectTrans.localPosition += new Vector3(0, yDelta, 0);
        //GetComponent<Text>().CrossFadeAlpha(0, alphaFadeDuration, true);
	}
}
