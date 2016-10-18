using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class ShopButton : MonoBehaviour {


    
    public GameObject SpA;
  //  public GameObject SpB;
  //  public GameObject SpC;

    public int cost;
    public GameObject saleItemA;

	public GameObject itemForSale;
	//public GameObject salceItemC;

   // [SyncVar]
    public float GrabDistance = 3.0f;


   public  bool SpawnA;
   public bool go;

    public float ItemAPrice;

    public Text ItemASign;
    public BankScript bank;
    // [SyncVar]
    RaycastHit Hit;
    // Use this for initialization

    List<GameObject> shopspawnitems;


    PlayerInventory player;
    void Start()
    {

        SpawnA = true;
        go = false;

        // ItemA = GameObject.Find("TestItem");
        //saleItemA = GameObject.FindGameObjectWithTag("ShopItemA");

        shopspawnitems = new List<GameObject>();
        //Instantiate(itemForSale, SpA.transform.position, SpA.transform.rotation);

        //   ItemA.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Pickupable>().enabled = true;

     //   Destroy(GameObject.FindGameObjectWithTag("ShopItemA"));
       
        ItemAPrice = 10;
        //    ItemASign.text = "$" + ItemAPrice.ToString();

        //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

      //  player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

     //   SpA.transform.position = GameObject.Find("ShipingLocation").transform.position;
    }

    // Update is called once per frame
    void Update () {
        //  buystuff();

       
       // Debug.Log(shopspawnitems[0]);

        if (GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay >= 0.75 && GameObject.Find("GameManager").GetComponent<DayNightController>().currentTimeOfDay <= 0.76)
        {
         //   GameObject.FindGameObjectsWithTag("Seed")[i].active = true;

          //  for (int i = 0; i < GameObject.FindGameObjectsWithTag("Seed").Length; i++)
           // {}
          //  shopspawnitems[0].active = true ;


            foreach (GameObject Item in shopspawnitems)
            {
                saleItemA.SetActive(true);

                Item.active = true;
            }

            shopspawnitems.Clear();

        }



        if (Input.GetMouseButtonDown(0))
            {
          
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

                Physics.Raycast(ray, out Hit, GrabDistance);

           
               

                if (Hit.collider.gameObject == GameObject.Find("ButtonForShop") && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().money >= cost)
                    {
                //      Instantiate(Resources.Load("TestItem"), SpA.transform.position, SpA.transform.rotation);

               

                GameObject newThing =  Instantiate(saleItemA);


                saleItemA.transform.position = GameObject.Find("ShipingLocation").transform.position;

                newThing.active = false;






              


               // newThing.GetComponent<Rigidbody>().MovePosition(GameObject.Find("ShipingLocation").transform.position);

                shopspawnitems.Add(newThing);

                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().money -= cost;
                GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Pickupable>().enabled = true;
                GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().isKinematic = false;

                Debug.Log("SHOP BUTTON CLICKED!");
                buystuff();


               


                    }
                
            }



	}


      public void buystuff()

    { 
   //    GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
   //    GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Pickupable>().enabled = true;


        for (int i = 0; i < GameObject.FindGameObjectsWithTag("ShopItemA").Length; i++)
        {
            if (Vector3.Distance(GameObject.FindGameObjectsWithTag("ShopItemA")[i].transform.position, SpA.transform.position) == 0 && GameObject.FindGameObjectWithTag("GameManager").GetComponent<BankScript>().Score >= 10)  // > 2 && SpawnA == true)
            {
                GameObject.FindGameObjectsWithTag("ShopItemA")[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GameObject.FindGameObjectsWithTag("ShopItemA")[i].GetComponent<Pickupable>().enabled = true;                
                Debug.Log("BLEH!");
                break;
            }

            if (Vector3.Distance(GameObject.FindGameObjectsWithTag("ShopItemA")[GameObject.FindGameObjectsWithTag("ShopItemA").Length-1].transform.position, SpA.transform.position) >= 2)   // != 0)
            {
                GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().isKinematic = false;              
               GameObject.FindGameObjectWithTag("GameManager").GetComponent<BankScript>().Score -= 10;

                GameObject newThing = Instantiate(saleItemA);

                saleItemA.transform.position = GameObject.Find("ShipingLocation").transform.position;

                newThing.active = false;

                
                //newThing.GetComponent<Rigidbody>().MovePosition(GameObject.Find("ShipingLocation").transform.position);

                shopspawnitems.Add(newThing);


                // Instantiate(Resources.Load("TestItem"), SpA.transform.position, SpA.transform.rotation);
                //GameObject.FindGameObjectsWithTag("ShopItemA")[i].GetComponent<Rigidbody>().isKinematic = false; //!!!!!
                GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.GetComponent<Rigidbody>().isKinematic = false;
                GameObject.FindGameObjectsWithTag("ShopItemA")[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GameObject.FindGameObjectsWithTag("ShopItemA")[i].GetComponent<Pickupable>().enabled = true;
                GameObject obj = GameObject.FindGameObjectWithTag("ShopItemA");
                Debug.Log("buying stuff: " + obj.name);
            }
        }
    }
    


       
    

}
