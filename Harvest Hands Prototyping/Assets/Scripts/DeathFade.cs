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


    private DayNightController DNCont;
    public float imageFadeInStartTime = 0.7f;
    [HideInInspector]
    public bool fadingIn = false;

    public Text deadText;
    public Text drownText;
    public float bedFadeInTime = 0.5f;
    public float morningFadeOutTime = 0.5f;

    public float deadTextFadeTime = 0.5f;
    public float deadTextShowDuration = 4f;
    
    [HideInInspector]
    public bool nightTimeWarning = false;
    public float nightTimeWarningTime = 0.7f;
    public Text nightTimeWarningText;
    public float nightWarningDisplayLength = 5f;

    public Image deathPenaltyImage;
    [FMODUnity.EventRef]
    public string deathSound = "event:/Done/Death Sound";

    [FMODUnity.EventRef]
    public string nightWarningSound = "event:/Done/Wolf Howl";
    [FMODUnity.EventRef]
    public string morningSound = "event:/Done/Rooster Howl";

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
            drownText.enabled = false;
            nightTimeWarningText.enabled = false;
        }
            deathPenaltyImage.enabled = false;

        FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        if (isLocalPlayer)
        {
            CmdFadeOut(true);
            deadText.CrossFadeAlpha(0, 0.01f, true);
            drownText.CrossFadeAlpha(0, 0.01f, true);
            nightTimeWarningText.CrossFadeAlpha(0, 0.01f, true);
            //deadText.color = new Vector4(deadText.color.r, deadText.color.g, deadText.color.b, 0);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        FadeImage = GameObject.Find("FadeImage").GetComponent<RawImage>();
        deadText = GameObject.Find("DeathText").GetComponent<Text>();
		drownText = GameObject.Find("DrownText").GetComponent<Text>();
		nightTimeWarningText = GameObject.Find("NightTimeWarningText").GetComponent<Text>();
        deathPenaltyImage = GameObject.Find("DeathPenaltyImage").GetComponent<Image>();
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
        if (nightTimeWarning == true)
        {            
            if (DNCont.currentTimeOfDay < nightTimeWarningTime)
            {
                nightTimeWarning = false;
                FMODUnity.RuntimeManager.PlayOneShot(morningSound, transform.position);
                
                Invoke("HideNightWarning", 0.01f);
            }            
        }

        //Begin Fade
        if (DNCont.currentTimeOfDay >= imageFadeInStartTime && fadingIn == false)
        {
            Debug.Log("Triggering fade in ");
            fadingIn = true;

            //              fade to 1, alpha == 1 at endDayAt
            if (DNCont.currentTimeOfDay < DNCont.endDayAt)
            {
                FadeImage.CrossFadeAlpha(1, DNCont.secondsInDay * (DNCont.endDayAt - imageFadeInStartTime), false);
            }
            else
                FadeImage.CrossFadeAlpha(1, bedFadeInTime, false);
        }
        //Begin Warning
        if (DNCont.currentTimeOfDay >= nightTimeWarningTime && nightTimeWarning == false)
        {
            nightTimeWarning = true;
            if (DNCont.currentTimeOfDay < DNCont.endDayAt)
            {
                FMODUnity.RuntimeManager.PlayOneShot(nightWarningSound, transform.position);
                nightTimeWarningText.CrossFadeAlpha(1, deadTextFadeTime, false);
                Invoke("HideNightWarning", nightWarningDisplayLength);
            }
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

    [Command]
    public void CmdBedFadeIn()
    {
        RpcBedFadeIn();
    }

    [ClientRpc]
    void RpcBedFadeIn()
    {
        if (!isLocalPlayer)
            return;

        fadingIn = true;
        if (DNCont.currentTimeOfDay > DNCont.endDayAt)
            FadeImage.CrossFadeAlpha(1, bedFadeInTime, false);
        else
        {
            FadeImage.CrossFadeAlpha(1, bedFadeInTime, false);
        }
    }

    [Command]
    public void CmdMorningFadeOut()
    {
        RpcMorningFadeOut();
    }

    [ClientRpc]
    void RpcMorningFadeOut()
    {
        if (!isLocalPlayer)
            return;

        FadeImage.CrossFadeAlpha(0, morningFadeOutTime, false);
    }

    [Command]
    public void CmdShowDeadText()
    {
        RpcShowDeadText();
    }

    [ClientRpc]
    public void RpcShowDeadText()
    {
        if (!isLocalPlayer)
            return;

        deadText.CrossFadeAlpha(1, deadTextFadeTime, false);
        Invoke("HideDeadText", deadTextShowDuration);
    }

    void HideDeadText()
    {
        deadText.CrossFadeAlpha(0, deadTextFadeTime, false);
    }

    [Command]
    public void CmdShowDrownText()
    {
        RpcShowDrownText();
    }

    [ClientRpc]
    public void RpcShowDrownText()
    {
        if (!isLocalPlayer)
            return;
                
        drownText.CrossFadeAlpha(1, deadTextFadeTime, false);
        Invoke("HideDrownText", deadTextShowDuration);
    }

    void HideDrownText()
    {
        drownText.CrossFadeAlpha(0, deadTextFadeTime, false);
    }

    [Command]
    public void CmdShowNightWarning()
    {
        RpcShowNightwarning();
    }

    [ClientRpc]
    public void RpcShowNightwarning()
    {
        if (!isLocalPlayer)
            return;

        nightTimeWarningText.CrossFadeAlpha(1, deadTextFadeTime, false);
        Invoke("HideNightWarning", nightWarningDisplayLength);
    }

    void HideNightWarning()
    {
        Debug.Log("Inside HideNightWarning()");
        nightTimeWarningText.CrossFadeAlpha(0, deadTextFadeTime, false);       //TO DO DIS
    }

    [Command]
    public void CmdHideNightTimeWarning()
    {
        CmdHideNightTimeWarning();
    }

    [ClientRpc]
    public void RpcHideNightTimeWarning()
    {

    }

    [Command]
    public void CmdSetShowDeathPenaltyImage(bool show)
    {
        RpcSetShowDeathPenaltyImage(show);
    }

    [ClientRpc]
    public void RpcSetShowDeathPenaltyImage(bool show)
    {
        if (!isLocalPlayer)
            return;

        deathPenaltyImage.enabled = show;
    }

    public void PlayDeathSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(deathSound, transform.position);
    }

    [ClientRpc]
    public void RpcPlayDeathSound()
    {
        if (!isLocalPlayer)
            return;

        FMODUnity.RuntimeManager.PlayOneShot(deathSound, transform.position);
    }
}
