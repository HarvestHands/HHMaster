using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class pauseMenu : MonoBehaviour {

  /*  public GameObject pauseCanvas;
    public bool isPaused;
	public GameObject localPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("escape") && isPaused == false)
        {

            //Time.timeScale = 0.0f;
            isPaused = true;
			localPlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().allowInput = false;

        }

       /* if (Input.GetKeyDown("escape") && isPaused == true)
        {

            Time.timeScale = 1.0f;
            pauseCanvas.GetComponent<Canvas>().enabled = false;
            isPaused = false;

        }
	}

    public void unPause()
    {

       // Time.timeScale = 1.0f;
        pauseCanvas.GetComponent<Canvas>().enabled = false;
        isPaused = false;
		localPlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().allowInput = true;
    }

    public void menuButton()
    {

        SceneManager.LoadScene(0);

    }

    public void quitGame()
    {

        Application.Quit();

    }
    */
}
