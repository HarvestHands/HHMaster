using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShopButton2 : NetworkBehaviour
{
    public GameObject ObjectToSpawn;
    public int cost;
    public Transform spawnPoint;
    public string gameManagerName = "GameManager";
    public BankScript bank;

    [FMODUnity.EventRef]
    public string buySound = "event:/Done/Gold Spend";

    // Use this for initialization
    void Start ()
    {
        bank = GameObject.Find(gameManagerName).GetComponent<BankScript>();
        if (ObjectToSpawn == null)
            Debug.Log(name + " has no ObjectToSpawn");

        GetComponent<Interactable>().onInteract += CmdBuyObj;
    }
	
    [Command]
    public void CmdBuyObj(NetworkInstanceId buyerID)
    {
        PlayerInventory playerInv = NetworkServer.FindLocalObject(buyerID).GetComponent<PlayerInventory>();
        if (playerInv.money >= cost)
        {
            playerInv.money -= cost;
            GameObject newSpawn = (GameObject)Instantiate(ObjectToSpawn, spawnPoint.position, spawnPoint.rotation);
            RpcPlaySound(buyerID);
        }
    }

    [ClientRpc]
    void RpcPlaySound(NetworkInstanceId id)
    {
        NetworkServer.FindLocalObject(id).GetComponent<PlayerInventory>().RpcPlaySoundForPlayer(buySound);
    }

}
