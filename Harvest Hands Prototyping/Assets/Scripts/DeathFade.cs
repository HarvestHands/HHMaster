using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DeathFade : NetworkBehaviour
{

    public RawImage FadeImage;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000; //texture order in hierarchy low = render on top,
    private float alpha = 1.0f;
    private float fadeDir= -1;

    public Text deadText;

    private DayNightController DNCont;
    public float imageFadeInStartTime = 0.7f;
    [HideInInspector]
    public bool fadingIn = false;

    // Use this for initialization
    void Start()
    {
        DNCont = FindObjectOfType<DayNightController>();
        
        if (FadeImage == null)
            Debug.Log("Player-FadeImage not assigned");

        if (!isLocalPlayer)
        {
            FadeImage.enabled = false;
            deadText.enabled = false;
        }

        FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        if (isLocalPlayer)
        {
            CmdFadeOut(true);
            deadText.CrossFadeAlpha(0, 0.01f, true);
            //deadText.color = new Vector4(deadText.color.r, deadText.color.g, deadText.color.b, 0);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        FadeImage = GameObject.Find("FadeImage").GetComponent<RawImage>();
        deadText = GameObject.Find("DeathText").GetComponent<Text>();
        DNCont = GameObject.FindObjectOfType<DayNightController>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (fadingIn == true)
        {
            if (DNCont.currentTimeOfDay < imageFadeInStartTime)
            {
                fadingIn = false;
            }
        }


        if (DNCont.currentTimeOfDay >= imageFadeInStartTime && fadingIn == false)
        {
            Debug.Log("Triggering fade in ");
            fadingIn = true;
            //              fade to 1, alpha == 1 at endDayAt
            FadeImage.CrossFadeAlpha(1, DNCont.secondsInDay * (DNCont.endDayAt - imageFadeInStartTime), false);
        }

    }

    [Command]
    void CmdFadeIn(bool isSafe)
    {
        RpcFadeIn(isSafe);
    }

    [ClientRpc]
    public void RpcFadeIn(bool isSafe)
    {
        if (!isLocalPlayer)
            return;
        //if died
        if (!isSafe)
        {
            deadText.CrossFadeAlpha(1, fadeSpeed, true);
            //deadTextFadeIn.Play();
        }

        //FadeImage.CrossFadeAlpha(1f, fadeSpeed, true);
    }

    [Command]
    void CmdFadeOut(bool isSafe)
    {
        RpcFadeOut(isSafe);
    }
    [ClientRpc]
    public void RpcFadeOut(bool isSafe)
    {
        if (!isLocalPlayer)
            return;

        transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Walking", false);
        //if (isSafe)
       //{
            deadText.CrossFadeAlpha(0, fadeSpeed, true);
        //}
        FadeImage.CrossFadeAlpha(0f, fadeSpeed, true);
    }

}
