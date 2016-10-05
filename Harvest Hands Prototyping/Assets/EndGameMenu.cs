using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public Text scoreText;
    public List<Text> highScoreTexts;
    public InputField inputField;
    private string playerName;
    private List<float> highscores;
    private List<string> highscoreNames;
    private float score;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(false);
        //enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void EndGameStuff(float finalScore)
    {
        score = finalScore;
        scoreText.text = "Final Score: " + finalScore.ToString();

        int numberOfHighScores = highScoreTexts.Count;

        //load old score order
        highscores = new List<float>();
        highscoreNames = new List<string>();

        //Update highscore texts
        for (int i = 0; i < numberOfHighScores; ++i)
        {
            highscores.Add(PlayerPrefs.GetFloat("finalScore" + i.ToString(), 0));
            //highscoreNames
            highScoreTexts[i].text = highscores[i].ToString();
        }

        SaveScores(numberOfHighScores);
    }

    public void SaveScores(int numberOfHighScores)
    {
        //if a new highscore, save it 
        if (score > PlayerPrefs.GetFloat("finalScore" + (numberOfHighScores - 1), 0))
        {
            //add new highscore to list
            highscores.Add(score);

            //calculate new highscore order
            highscores.Sort((x, y) => y.CompareTo(x));

            //Save new list order
            for (int i = 0; i < numberOfHighScores; ++i)
            {
                PlayerPrefs.SetFloat("finalScore" + i.ToString(), highscores[i]);
                Debug.Log("finalScore" + i.ToString() + " = " + highscores[i].ToString());
            }
            //RenderSettings.skybox.color = Color.red;
        }
    }

    public void DeleteScores()
    {
        PlayerPrefs.DeleteAll();
    }
   
}
