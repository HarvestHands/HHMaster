using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EndGameMenu : MonoBehaviour
{
    public Text scoreText;
    public List<Text> highScoreTexts;
    public InputField inputField;
    private string playerName;
    public List<float> highscores;
    public List<string> highscoreNames;
    private float score;
    private bool scoreSaved = false;

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
            highscoreNames.Add(PlayerPrefs.GetString("finalScoreName" + i.ToString(), "Farmer"));

            if (highScoreTexts[i] != null)
                highScoreTexts[i].text = highscoreNames[i].ToString() + ": " + highscores[i].ToString();
        }

        //SaveScores(numberOfHighScores);
        if (score > PlayerPrefs.GetFloat("finalScore" + (numberOfHighScores - 1), 0))
        {
            inputField.ActivateInputField();
        }
    }

    public void SaveScores()
    {
        //only save once
        if (scoreSaved)
            return;
        
        //add new highscore to list
        highscores.Add(score);
        highscoreNames.Add(inputField.text);

        //calculate new highscore order
        //highscores.Sort((x, y) => y.CompareTo(x));
        bool listSorted = false;
        while (!listSorted)
        {
            listSorted = true;
            for (int i = 0; i < highscores.Count - 1; ++i)
            {
                
                if (highscores[i + 1] > highscores[i])
                {
                    float temp = highscores[i];
                    highscores[i] = highscores[i + 1];
                    highscores[i + 1] = temp;

                    string tempName = highscoreNames[i];
                    highscoreNames[i] = highscoreNames[i + 1];
                    highscoreNames[i + 1] = tempName;

                    listSorted = false;
                }
            }
        }

        //Save new list order
        for (int i = 0; i < highscores.Count - 1; ++i)
        {
            //Debug.Log("saving scores = score - " + highscores[i].ToString() + ", name - " + highscoreNames[i].ToString());
            PlayerPrefs.SetFloat("finalScore" + i.ToString(), highscores[i]);
            PlayerPrefs.SetString("finalScoreName" + i.ToString(), highscoreNames[i]);
            //Debug.Log("finalScore" + i.ToString() + " = " + highscores[i].ToString());
        }
          
        scoreSaved = true;
    }

    public void DeleteScores()
    {
        PlayerPrefs.DeleteAll();
    }

    public void StopHost()
    {
        NetworkManager.singleton.StopHost();
    }
   
}
