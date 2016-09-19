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

    // Use this for initialization
    void Start()
    {
        if (FadeImage == null)
            Debug.Log("Player-FadeImage not assigned");

        if (!isLocalPlayer)
        {
            FadeImage.enabled = false;
            deadText.enabled = false;
            //deadText.color = new Vector4(deadText.color.r, deadText.color.g, deadText.color.b, 0);
        }
        //else
        //FadeImage.rectTransform.rect 
        //FadeImage.uvRect.width = Screen.width;//.size.x =  = new Vector2(Screen.width, Screen.height);

        FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        if (isLocalPlayer)
        {
            CmdFadeOut(true);
            //deadText.color = new Vector4(deadText.color.r, deadText.color.g, deadText.color.b, 0);
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

        FadeImage.CrossFadeAlpha(1f, fadeSpeed, true);
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

        if (!isSafe)
        {
            deadText.CrossFadeAlpha(0, fadeSpeed, true);
        }
        FadeImage.CrossFadeAlpha(0f, fadeSpeed, true);
    }

}
