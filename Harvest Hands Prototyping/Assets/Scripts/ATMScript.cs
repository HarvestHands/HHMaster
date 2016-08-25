using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ATMScript : NetworkBehaviour
{

    [Tooltip("Withdraw = true, deposit = false")]
    public bool ATMType = false;
    public int amount = 30;

    BankScript farmbank;

    public override void OnStartClient()
    {
        base.OnStartClient();
        farmbank = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BankScript>();
    }

    [Command]
    public void CmdWithdrawMoney(NetworkInstanceId playerNetworkId)
    {
        GameObject player = NetworkServer.FindLocalObject(playerNetworkId);
        PlayerInventory playerInv = player.GetComponent<PlayerInventory>();

        Debug.Log("inside CmdWithdrawMoney");

        //withdraw
        if (farmbank.Score >= amount)
        {
            //RpcGivePlayerMoney(playerNetworkId, amount);
            playerInv.RpcGiveMoney(amount);
            farmbank.Score -= amount;
        }
        //if farmbank < amount, withdraw remaining
        else
        {
            //RpcGivePlayerMoney(playerNetworkId, farmbank.Score);
            playerInv.RpcGiveMoney(farmbank.Score);
            farmbank.Score = 0;
        }      
    }

    [Command]
    public void CmdDepositMoney(NetworkInstanceId playerNetworkId)
    {
        GameObject player = NetworkServer.FindLocalObject(playerNetworkId);
        PlayerInventory playerInv = player.GetComponent<PlayerInventory>();

        //depositing
        if (playerInv.money >= amount)
        {
            playerInv.RpcGiveMoney(-amount);
            farmbank.Score += amount;
        }
        //if playerMoney < amount, deposit remaining
        else
        {
            farmbank.Score += playerInv.money;
            playerInv.RpcGiveMoney(-playerInv.money);
        }
    }

    /*

    [Command]
    public void CmdRequestMoney(NetworkInstanceId playerNetworkId)
    {
        GameObject player = NetworkServer.FindLocalObject(playerNetworkId);
        PlayerInventory playerInv = player.GetComponent<PlayerInventory>();

        //withdraw
        if (ATMType)
        {
            if (farmbank.Score >= amount)
            {
                //RpcGivePlayerMoney(playerNetworkId, amount);
                playerInv.RpcGiveMoney(amount);
                farmbank.Score -= amount;
            }
            //if farmbank < amount, withdraw remaining
            else
            {
                //RpcGivePlayerMoney(playerNetworkId, farmbank.Score);
                playerInv.RpcGiveMoney(farmbank.Score);
                farmbank.Score = 0;
            }
        }
        //depositing
        else
        {
            if (playerInv.money >= amount)
            {
                playerInv.RpcGiveMoney(-amount);
                farmbank.Score += amount;
            }
            //if playerMoney < amount, deposit remaining
            else
            {
                farmbank.Score += playerInv.money;
                playerInv.RpcGiveMoney(-playerInv.money);
            }
        }
    }


    [ClientRpc]
    public void RpcGivePlayerMoney(NetworkInstanceId playerNetworkID, int _amount)
    {
        GameObject player = ClientScene.FindLocalObject(playerNetworkID);
        player.GetComponent<PlayerInventory>().RpcGiveMoney(_amount);    
    }

    [ClientRpc]
    public void RpcTakePlayerMoney(NetworkInstanceId playerNetworkID, int _amount)
    {
        GameObject player = ClientScene.FindLocalObject(playerNetworkID);
        player.GetComponent<PlayerInventory>().RpcGiveMoney(-_amount);
    }
    */
}
