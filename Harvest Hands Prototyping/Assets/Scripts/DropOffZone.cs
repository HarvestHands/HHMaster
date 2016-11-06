using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DropOffZone : NetworkBehaviour
{ 
    GameObject gameManager;
    //ShopScript shop;
    BankScript farmbank;
    
    public float scoreMultiplier = 1;

    // Use this for initialization
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        farmbank = gameManager.GetComponent<BankScript>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Produce"))
        {            
            //Make Command?
            PlantProduce produce = col.gameObject.GetComponent<PlantProduce>();
            //shop.Score += produce.score;
            farmbank.RpcSpawnPriceText(produce.score);
            farmbank.Score += produce.score * produce.ProduceAmount;
            Destroy(produce.gameObject);
        }
        //else if (col.gameObject.CompareTag("Mushroom"))
        //{
        //    Mushroom mushroom = col.gameObject.GetComponent<Mushroom>();
        //    farmbank.Score += mushroom.score;
        //    Destroy(mushroom.gameObject);
        //}
    }
}
