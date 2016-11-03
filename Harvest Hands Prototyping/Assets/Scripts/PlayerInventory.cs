using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInventory : NetworkBehaviour {

    public bool isSafe;

    BankScript farmBank;
    ShopScript shop;
    public Text playerMoneyText;
    public Text farmMoneyText;
    
    public int money = 0;
    private int oldMoney = -1;
    [Tooltip("0 = 0% lost, 0.4 = 40% lost, 1 = 100% lost")]    
    public float deathPenalty = 0.2f;

    public Canvas UICanvas;
    public Text costText;
    public float costTextLifeTime = 2.5f;

    [FMODUnity.EventRef]
    public string depositSound = "event:/Done/Gold Spend";
    [FMODUnity.EventRef]
    public string withdrawSound = "event:/Done/Gold income";

    // Use this for initialization
    void Awake ()
    {
        shop = GameObject.Find("GameManager").GetComponent<ShopScript>();
        farmBank = GameObject.Find("GameManager").GetComponent<BankScript>();
        if (isLocalPlayer)
        {
            //playerMoneyText = GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
			playerMoneyText = GameObject.Find("PlayerMoneyText").GetComponent<Text>();

			Debug.Log ("UICanvas = " + UICanvas);
            //Debug.Log("FarmBank " + farmBank);
            //Debug.Log("FarmMoneyText " + farmMoneyText);
            farmBank.localPlayerFarmMoneyText = farmMoneyText;
            farmMoneyText.text = farmBank.Score.ToString();
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		shop = GameObject.Find("GameManager").GetComponent<ShopScript>();
		farmBank = GameObject.Find("GameManager").GetComponent<BankScript>();

		playerMoneyText = GameObject.Find("PlayerMoneyText").GetComponent<Text>();
		farmMoneyText = GameObject.Find ("FarmMoneyText").GetComponent<Text> ();
		UICanvas = GameObject.Find("UI").GetComponent<Canvas> ();
		farmBank.localPlayerFarmMoneyText = farmMoneyText;
		farmMoneyText.text = farmBank.Score.ToString();
		GetComponentInChildren<MeshRenderer>().enabled = false;
	}



	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (oldMoney != money)
        {
            playerMoneyText.text = "$" + money.ToString();
            SpawnPriceText(money - oldMoney);
            oldMoney = money;
        }

 //       if(Input.GetKeyDown(KeyCode.Q))
 //       {
 //           //spawns seed infront of player
 //           //shop.CmdSpawnSeeds(transform.position + transform.forward * 2, transform.rotation);
 //
 //           CmdSpawnSeeds();
 //       }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 4))
        {
            ATMScript hitAtm = Hit.collider.GetComponent<ATMScript>();
            if (hitAtm)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Debug.Log("Withdraw Called");
                    //hitAtm.CmdWithdrawMoney(netId);
                    CmdWithdrawMoneyFromFarm(hitAtm.amount);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Debug.Log("Deposit Called");
                    //hitAtm.CmdDepositMoney(netId);
                    CmdDepositMoneyToFarm(hitAtm.amount);
                }
            }
        }        
    }

    [Command]
    void CmdSpawnSeeds()
    {
        shop.Score--;
        //create seeds
        GameObject seeds = (GameObject)Instantiate(shop.seedsPrefab, transform.position, transform.rotation);
        seeds.transform.position += transform.forward * 2;

        //spawn on clients
        NetworkServer.Spawn(seeds);
    }

    [ClientRpc]
    public void RpcGiveMoney(int amount)
    {
        Debug.Log("RPCGiveMoneyCalled");
        money += amount;
    }


    [Command]
    public void CmdDepositMoneyToFarm(int amount)
    {
        //depositing
        if (money >= amount)
        {
            farmBank.CmdDepositMoney(netId, amount);
        }
        //if playerMoney < amount, deposit remaining
        else
        {
            farmBank.CmdDepositMoney(netId, money);
        }

        //Play Sound
        FMODUnity.RuntimeManager.PlayOneShot(depositSound, transform.position);

        //farmBank.CmdDepositMoney(netId, amount);
    }

    [Command]
    public void CmdWithdrawMoneyFromFarm(int amount)
    {
        //Withdraw
        if (farmBank.Score >= amount)
        {
            farmBank.CmdWithdrawMoney(netId, amount);
            RpcPlaySoundForPlayer(withdrawSound);            
        }
        //if playerMoney < amount, deposit remaining
        else
        {
            farmBank.CmdWithdrawMoney(netId, farmBank.Score);
        }

        //Play Sound
        FMODUnity.RuntimeManager.PlayOneShot(withdrawSound, transform.position);

        //farmBank.CmdWithdrawMoney(netId, amount);
    }


    [ClientRpc]
    public void RpcPlaySoundForPlayer(string sound)
    {
        //Debug.Log("Inside player play sound");
        if (isLocalPlayer)
        {
            //Debug.Log("Playing sound for local player");
            //Play Sound
            FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
        }
    }

    public void SpawnPriceText(int price)
    {
        if (!isLocalPlayer)
            return;

        if (UICanvas != null)
        {
            Text priceText = (Text)Instantiate(costText, playerMoneyText.rectTransform.position, playerMoneyText.rectTransform.rotation);
			priceText.rectTransform.SetParent(UICanvas.transform, false); 
            priceText.rectTransform.position = playerMoneyText.rectTransform.position;
            priceText.rectTransform.rotation = playerMoneyText.rectTransform.rotation;

<<<<<<< HEAD
            //if (price > 0)
            //    priceText.text = "-";
            //else
            //    priceText.text = "+";
            priceText.text = "$" + price; 
=======
            if (price > 0)
                priceText.text = "-";
            else
                priceText.text = "+";
            priceText.text += "$" + price; 
>>>>>>> c569af3dbb46c75d6b3a9904dcd8970479a8fd4e

            Destroy(priceText.gameObject, costTextLifeTime);
        }
    }

    [ClientRpc]
    public void RpcApplyDeathPenalty()
    {
        money -= Mathf.CeilToInt(money * deathPenalty);
    }

}
