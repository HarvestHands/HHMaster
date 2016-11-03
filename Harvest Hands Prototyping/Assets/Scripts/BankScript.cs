using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BankScript : NetworkBehaviour
{
    [SyncVar]
    public int Score = 0;
    public int oldScore = -1;

    public Text localPlayerFarmMoneyText;

    public Canvas UICanvas;
    public Text costText;
    public float costTextLifeTime = 2.5f;

    public string playerTag = "Player";
    

    void Start()
    {
        
    }

    void Update()
    {

        //RpcStore();
        
        if (oldScore != Score)
        {
            Debug.Log("inside oldscore != score");
            //SpawnPriceText(Score - oldScore);
            oldScore = Score;
            if (localPlayerFarmMoneyText)
                localPlayerFarmMoneyText.text = "$" + Score.ToString();
            //Do stuff

        }
    }

    [Command]
    public void CmdDepositMoney(NetworkInstanceId playerNetworkId, int amount)
    {
        Score += amount;
        RpcGivePlayerMoney(playerNetworkId, -amount);
    }

    [Command]
    public void CmdWithdrawMoney(NetworkInstanceId playerNetworkId, int amount)
    {
        Score -= amount;
        RpcGivePlayerMoney(playerNetworkId, amount);
    }

    [ClientRpc]
    public void RpcGivePlayerMoney(NetworkInstanceId playerId, int amount)
    {
        ClientScene.FindLocalObject(playerId).GetComponent<PlayerInventory>().money += amount;
        
    }

    [ClientRpc]
    public void RpcSpawnPriceText(int price)
    {
        SpawnPriceText(price);
    }

    public void SpawnPriceText(int price)
    {
        Debug.Log("Inside Bankscript Spawn Price Text");
        

        if (UICanvas != null)
        {
            Text priceText = (Text)Instantiate(costText, localPlayerFarmMoneyText.rectTransform.position, localPlayerFarmMoneyText.rectTransform.rotation);
            priceText.rectTransform.SetParent(UICanvas.transform, false);
            priceText.rectTransform.position = localPlayerFarmMoneyText.rectTransform.position;
            priceText.rectTransform.rotation = localPlayerFarmMoneyText.rectTransform.rotation;
            
            //if (price > 0)
            //    priceText.text = "-";
            //else
            //    priceText.text = "+";
            priceText.text = "$" + price;
            
            Destroy(priceText.gameObject, costTextLifeTime);
        }
    }

}
