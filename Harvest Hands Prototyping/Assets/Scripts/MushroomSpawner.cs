using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MushroomSpawner : NetworkBehaviour
{
    [System.Serializable]
    public class MushroomInfo
    {
        public GameObject mushroom;
        public float spawnChance;
    }

    public List<MushroomInfo> mushrooms;
    public float chanceToSpawn = 40;
    [SyncVar]
    public bool canSpawn = true;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.T))
        {
            AttemptSpawn();
        }
	}

    public void AttemptSpawn()
    {
        if (canSpawn)
        {
            float chance = Random.Range(0, 100);
            if (chance <= chanceToSpawn)
            {
                if (mushrooms.Count < 1)
                {
                    Debug.Log(name + " - mushroom list is empty");
                    return;
                }
                float randNum = Random.Range(0, 100);
                float spwn = 0;
                int mushIndex = 0;
                for (int i = 0; i < mushrooms.Count; ++i)
                {
                    spwn += mushrooms[i].spawnChance;
                    if (spwn >= randNum)
                    {
                        mushIndex = i;
                        Debug.Log("spawn: " + spwn + " < randnum" + randNum + " --- i = " + i);
                        break;
                    }

                }

                //int mushIndex = Random.Range(0, mushrooms.Count - 1);
                GameObject newMush = (GameObject)Instantiate(mushrooms[mushIndex].mushroom, transform.position, transform.rotation);
                newMush.GetComponent<Mushroom>().mushroomSpawner = gameObject;
                NetworkServer.Spawn(newMush);
                canSpawn = false;
                //Debug.Log(randNum + " = Mushroom: " + mushIndex);
            }
        }
        else
        {
            //Debug.Log("Canspawn = false");
        }
    }



}
