using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopScript1 : NetworkBehaviour
{
    public GameObject seedsPrefab;

    [SyncVar]
    public int Score = 0;
    public int oldScore = 0;

    public Text playerUI;

    GameObject StoreMenu;  
    GameObject Storekeep;
    GameObject TargetPlayer;

    public Quaternion rotation2 = Quaternion.Euler(new Vector3(1, 1, 1));

   // Vector3 SpawnLoc = new Vector3(790.27f, 82.63f, 1102.5f);


    [SyncVar]
    Vector3 SpawnLoc;


    [SyncVar]
    float distance;


    [SyncVar]
    bool playerCloseEnough;

    GameObject[] allplayers;
    GameObject[] allseeds;
    

    private NetworkIdentity StoreKeepNetId;


	// Use this for initialization
	void Start ()
    {
        oldScore = Score - 1;

        StoreMenu = GameObject.Find("StoreMenu");
        TargetPlayer = GameObject.FindGameObjectWithTag("Player");
        Storekeep = GameObject.Find("StoreKeep");

        Renderer rend = Storekeep.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");

        SpawnLoc = Storekeep.transform.position;
        Storekeep.GetComponent<Renderer>().sharedMaterials[0].color = Color.grey;
        Storekeep.GetComponent<Renderer>().sharedMaterials[1].color = Color.grey;

        playerCloseEnough = false;
        distance = 10;

        allseeds = GameObject.FindGameObjectsWithTag("Seed");
        
	}

	
	// Update is called once per frame
    void Update()
    {
  
      //RpcStore();

            if (oldScore != Score)
            {
                //GameObject.FindGameObjectsWithTag("Player");
                if (playerUI)
                    playerUI.text = "Score: " + Score;

                oldScore = Score;
                //Do stuff

            }
        
    }


    public override void OnStartLocalPlayer()
    { }


    [Command]
    public void CmdSpawnSeeds(Vector3 position, Quaternion rotation)
    {
        //create seeds
        GameObject seeds = (GameObject)Instantiate(seedsPrefab, position, rotation);
        seeds.transform.position += transform.forward * 2;

        //spawn on clients
        NetworkServer.Spawn(seeds);     
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
   
    
    }


    [ClientRpc]
    void RpcClientkey(KeyCode x)
    {
        //netid 38
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(x))
            {
                CmdSpawnSeeds(SpawnLoc, rotation2);           
                DestroyObject(allseeds[allseeds.Length]);
            }
        }
    }


    [RPC]
    void Rpckey(KeyCode x)
    {

        if (!isLocalPlayer)
        {
            if (Input.GetKeyDown(x))
            {
                CmdSpawnSeeds(SpawnLoc, rotation2);
                 DestroyObject( GameObject.Find("Seeds(Clone)"));      
            }
        }
    } 




    [ClientRpc]
    public void RpcStore()
    {
        allplayers = GameObject.FindGameObjectsWithTag("Player");

      //  int j = 0;

        for (int i = 0; i < allplayers.Length; i++)
        {
            if (Vector3.Distance(allplayers[0].transform.position, Storekeep.transform.position) < distance)
            {
                Storekeep.GetComponent<Renderer>().sharedMaterials[0].color = Color.green;
                Storekeep.GetComponent<Renderer>().sharedMaterials[1].color = Color.green;
                Rpckey(KeyCode.R);        
            }



            if (allplayers.Length == 1)
            {

                if (Vector3.Distance(allplayers[0].transform.position, Storekeep.transform.position) < distance && isLocalPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        CmdSpawnSeeds(SpawnLoc, rotation2);
              
                    }
                }


                if (Vector3.Distance(allplayers[0].transform.position, Storekeep.transform.position) > distance)
                {
                    Storekeep.GetComponent<Renderer>().sharedMaterials[0].color = Color.grey;
                    Storekeep.GetComponent<Renderer>().sharedMaterials[1].color = Color.grey;

                }
            }


            if (allplayers.Length == 2)
            {         
                if (Vector3.Distance(allplayers[1].transform.position, Storekeep.transform.position) < distance && !isLocalPlayer)
                {
                    Storekeep.GetComponent<Renderer>().sharedMaterials[0].color = Color.green;
                    Storekeep.GetComponent<Renderer>().sharedMaterials[1].color = Color.green;
                    RpcClientkey(KeyCode.R);                  
                }


                if (Vector3.Distance(allplayers[1].transform.position, Storekeep.transform.position) > distance && (Vector3.Distance(allplayers[0].transform.position, Storekeep.transform.position) > distance))
                {
                    Storekeep.GetComponent<Renderer>().sharedMaterials[0].color = Color.grey;
                    Storekeep.GetComponent<Renderer>().sharedMaterials[1].color = Color.grey;

                }


                if (Vector3.Distance(allplayers[1].transform.position, Storekeep.transform.position) < distance && (Vector3.Distance(allplayers[0].transform.position, Storekeep.transform.position) < distance))
                {}

                }
            }
        }
    }
        
        



