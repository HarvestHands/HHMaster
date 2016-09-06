using UnityEngine;
using System.Collections;

public class pickupable2 : MonoBehaviour
{
    public enum pickupType
    {
        PICKUP = 0,
        INTERACT = 1,
        BUY = 2,
    }


    public bool hit;
    //private Texture tex;
    public pickupType type;
    public float FresnelAmount;
    public float FresnelDecrease;
    //public float FresnalRate;
    public float FresnelIncrease;
    //private float FresnalTimer;
    // Use this for initialization
    void Start()
    {
		GetComponent<Renderer> ().sharedMaterial = GetComponent<Renderer> ().material;



        //Debug.Log(tex);

        //FresnelDrop = 0.1f; // drop amount per rate
        //FresnalRate = 0.1f; // seconds
        //FresnelIncrease = 0.5f; // increase amount per rate

        //FresnalTimer = FresnalRate;
    }

    // Update is called once per frame
    void Update()
    {        
        if (hit)
        {
 
            FresnelAmount += Time.deltaTime * FresnelIncrease;
            FresnelAmount = Mathf.Clamp(FresnelAmount, 0, 1);
            //GetComponent<Renderer>().sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);
            switch (type)
            {
                case pickupType.PICKUP:
                    GetComponent<Renderer>().sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.PickUp);
                    GetComponent<Renderer>().sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
                case pickupType.INTERACT:
                    GetComponent<Renderer>().sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.Interact);
                    GetComponent<Renderer>().sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
                case pickupType.BUY:
                    GetComponent<Renderer>().sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.Buy);
                    GetComponent<Renderer>().sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
            }
        }
        else
        {
            FresnelAmount -= Time.deltaTime * FresnelDecrease;
            FresnelAmount = Mathf.Clamp(FresnelAmount, 0, 1);
            GetComponent<Renderer>().sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);
        }

        hit = false;
    }
}
