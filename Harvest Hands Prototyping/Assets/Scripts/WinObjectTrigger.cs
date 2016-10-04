using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WinObjectTrigger : MonoBehaviour
{
    public bool gameWon = false;
    public int numberOfHighScores = 5;
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



        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        

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
            //if a new highscore
            if (finalScore > PlayerPrefs.GetFloat("finalScore" + (numberOfHighScores - 1), 0))
            {
                //load old score order
                List<float> highscores = new List<float>();
                for (int i = 0; i < numberOfHighScores; ++i)
                {
                    highscores.Add(PlayerPrefs.GetFloat("finalScore" + i.ToString(), 0));
                    
                }
                //add new highscore to list
                highscores.Add(finalScore);

                //calculate new highscore order
                highscores.Sort((x, y) => y.CompareTo(x));

                //Save new list order
                for (int i = 0; i < numberOfHighScores; ++i)
                {
                    PlayerPrefs.SetFloat("finalScore" + i.ToString(), highscores[i]);
                    Debug.Log("finalScore" + i.ToString() + " = " + highscores[i].ToString());
                }
            }


        }
    }
}
