using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DropOffZone : NetworkBehaviour {

    GameObject gameManager;
    ShopScript shop;
    BankScript farmbank;
    
    public float scoreMultiplier = 1;


    // Use this for initialization
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        shop = gameManager.GetComponent<ShopScript>();
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
            shop.Score += produce.score;
            farmbank.Score += produce.score;
            Destroy(produce.gameObject);
        }
    }
}
