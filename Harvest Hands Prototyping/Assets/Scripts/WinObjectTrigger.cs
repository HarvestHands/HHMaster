using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WinObjectTrigger : NetworkBehaviour
{
    public GameObject gameOverCanvasObject;

    public bool gameWon = false;
    //public int numberOfHighScores = 5;
    public float finalScore = 0;
    // Use this for initialization
    void Awake ()
    {
        Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!gameWon)
        {
            finalScore += Time.deltaTime;
            return;
        }



       /* if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }*/
        

	}

    void OnTriggerEnter(Collider col)
    {
        //if collider is WinObject
        if (col.GetComponent<WinObject>() != null)
        {
            //Win Game
            gameWon = true;
            Time.timeScale = 0.0001f;
            //int finalScore = 1;

            //PlayerPrefs.SetFloat("finalScore1", finalScore);
            //Debug.Log(gameOverCanvasObject.name + " - trying to set active");
            gameOverCanvasObject.SetActive(true);
            gameOverCanvasObject.GetComponent<Canvas>().enabled = true;
            //Debug.Log(gameOverCanvasObject.active + " = active?");
            
            UnityStandardAssets.Characters.FirstPerson.FirstPersonController[] Players = GameObject.FindObjectsOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            foreach (UnityStandardAssets.Characters.FirstPerson.FirstPersonController player in Players)
            {
                player.allowInput = false;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            gameOverCanvasObject.GetComponent<EndGameMenu>().EndGameStuff(finalScore);
        }
    }


    [Command]
    void CmdLockPlayersAndShowCursors()
    {
        RpcLockPlayersAndShowCursors();
    }

    [ClientRpc]
    void RpcLockPlayersAndShowCursors()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController[] Players = GameObject.FindObjectsOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        foreach (UnityStandardAssets.Characters.FirstPerson.FirstPersonController player in Players)
        {
            player.allowInput = false;
        }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }

}
