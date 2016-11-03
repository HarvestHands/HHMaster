using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class TutorialScript : MonoBehaviour
{
    //on client start activate?
    public int show = 0;
    
    public List<GameObject> list;
    private int currentIndex;

    [Header("Optional")]
    public KeyCode skipKey = KeyCode.None;
	public KeyCode nextKey = KeyCode.None;

	// Use this for initialization
	void Start ()
    {
        foreach (GameObject go in list)
        {
            go.SetActive(false);
        }

        list[currentIndex].SetActive(true);        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if there is a skip key
	    if (skipKey != KeyCode.None)
        {
            if(Input.GetKeyDown(skipKey))
            {
                gameObject.SetActive(false);
            }
        }

		if (Input.GetKeyDown(nextKey))
        {
            NextGO();
        }
	}
    
    public void NextGO()
    {
        //if still more text
        if (currentIndex < list.Count - 1)
        {
            list[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            list[currentIndex].gameObject.SetActive(true);
        }
        //no more text
        else
        {
            list[currentIndex].gameObject.SetActive(false);

            //turn off canvas
            gameObject.SetActive(false);
        }
    }
}
