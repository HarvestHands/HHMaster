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

    public string playerTag = "Player";
    

    void Start()
    {
        SaveAndLoad.SaveEvent += SaveFunction;
    }

    void OnDestroy()
    {
        SaveAndLoad.SaveEvent -= SaveFunction;
    }

    void Update()
    {

        //RpcStore();
        
        if (oldScore != Score)
        {
            oldScore = Score;
            if(localPlayerFarmMoneyText)
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

    public void SaveFunction(object sender, string args)
    {
        SavedGameManager GM = new SavedGameManager();
        //Day Night Controller
        DayNightController DNC = GetComponent<DayNightController>();
        GM.ingameDay = DNC.ingameDay;
        GM.currentTimeOfDay = DNC.currentTimeOfDay;
        GM.dayNightCheckDone = DNC.nightTimeCheckDone;
        //Bank
        GM.score = Score;

        SaveAndLoad.localData.savedGameManager.Add(GM);
    }


}
