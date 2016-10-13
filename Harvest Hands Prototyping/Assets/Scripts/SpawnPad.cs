using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnPad : NetworkBehaviour
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
	}
	
    [Command]
    public void CmdSpawnObject(NetworkInstanceId id)
    {
        PlayerInventory player = NetworkServer.FindLocalObject(id).GetComponent<PlayerInventory>();
        if (player.money >= cost)
        {            
            player.money -= cost;
            //bank.Score -= cost;
            GameObject newSpawn = (GameObject)Instantiate(ObjectToSpawn, spawnPoint.position, transform.rotation);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CmdSpawnObject(col.GetComponent<NetworkIdentity>().netId);
            RpcPlaySound(col.GetComponent<NetworkIdentity>().netId);
        }
    }

    [ClientRpc]
    void RpcPlaySound(NetworkInstanceId id)
    {
        NetworkServer.FindLocalObject(id).GetComponent<PlayerInventory>().RpcPlaySoundForPlayer(buySound);
    }

}
