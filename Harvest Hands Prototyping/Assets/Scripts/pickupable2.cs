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

    [Header("Optional")]
    public Renderer renderer;
    // Use this for initialization
    void Start()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();

            //GetComponent<Renderer> ().sharedMaterial = GetComponent<Renderer> ().material;

        renderer.sharedMaterial = renderer.material;



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
                    renderer.sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.PickUp);
                    renderer.sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
                case pickupType.INTERACT:
                    renderer.sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.Interact);
                    renderer.sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
                case pickupType.BUY:
                    renderer.sharedMaterial.SetColor("_FresnelColour", ColorSingleton.instance.Buy);
                    renderer.sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);// FresnelAmount
                    break;
            }
        }
        else
        {
            FresnelAmount -= Time.deltaTime * FresnelDecrease;
            FresnelAmount = Mathf.Clamp(FresnelAmount, 0, 1);
            renderer.sharedMaterial.SetFloat("_FresnelAmount", FresnelAmount);
        }

        hit = false;
    }
}
