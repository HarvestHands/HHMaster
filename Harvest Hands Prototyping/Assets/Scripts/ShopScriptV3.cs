using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopScriptV3 : MonoBehaviour {



    public GameObject SpA;
    public GameObject SpB;
    public GameObject SpC;


   public GameObject ItemA;

   // [SyncVar]
    public float GrabDistance = 3.0f;


  public  bool SpawnA;
   public bool go;

    public float ItemAPrice;

    public Text ItemASign;

    // [SyncVar]
    RaycastHit Hit;
	// Use this for initialization
	void Start () {

        SpawnA = true;
        go = false;

     //   ItemA = GameObject.Find("TestItem");
        ItemA = GameObject.FindGameObjectWithTag("ShopItemA");

        Instantiate(Resources.Load("TestItem"), SpA.transform.position, SpA.transform.rotation);

        //   ItemA.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GameObject.FindGameObjectWithTag("ShopItemA").GetComponent<Pickupable>().enabled = true;




        ItemAPrice = 10;
    //    ItemASign.text = "$" + ItemAPrice.ToString();






    }

    // Update is called once per frame
    void Update()
    {
        //  if (GameObject.FindGameObjectWithTag("Player").GetComponent<StaffNo3>().ChosenObj.tag == "ShopItemA")
        //  { }


        buystuff();
       
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

                Instantiate(Resources.Load("TestItem"), SpA.transform.position, SpA.transform.rotation);



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

       
    



