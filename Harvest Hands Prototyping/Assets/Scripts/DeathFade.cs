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

    // Use this for initialization
    void Start()
    {
        if (FadeImage == null)
            Debug.Log("Player-FadeImage not assigned");

        if (!isLocalPlayer)
            FadeImage.enabled = false;
        //else
        //FadeImage.rectTransform.rect 
        //FadeImage.uvRect.width = Screen.width;//.size.x =  = new Vector2(Screen.width, Screen.height);

        FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        if(isLocalPlayer)
            CmdFadeOut();
        
    }

    [Command]
    void CmdFadeIn()
    {
        RpcFadeIn();
    }

    [ClientRpc]
    public void RpcFadeIn()
    {
        if (!isLocalPlayer)
            return;

        FadeImage.CrossFadeAlpha(1f, fadeSpeed, true);
    }

    [Command]
    void CmdFadeOut()
    {
        RpcFadeOut();
    }
    [ClientRpc]
    public void RpcFadeOut()
    {
        if (!isLocalPlayer)
            return;

        FadeImage.CrossFadeAlpha(0f, fadeSpeed, true);
    }

}
