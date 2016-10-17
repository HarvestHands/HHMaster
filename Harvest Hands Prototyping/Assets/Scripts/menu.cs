using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class menu : MonoBehaviour {

    public GameObject settingsCanvas;
    public GameObject menuCanvas;
    Resolution[] resolutions;
    public Dropdown resolutionDrop;
    public Dropdown qualityDrop;

public void singlePlayer()
    {

        SceneManager.LoadScene(2);
        Debug.Log("singleplayer");

    }
		

public void settings()
    {

        settingsCanvas.GetComponent<Canvas>().enabled=true;
        menuCanvas.GetComponent<Canvas>().enabled = false;
        Debug.Log("settings");

    }

public void menuButton()
    {

        menuCanvas.GetComponent<Canvas>().enabled = true;
        settingsCanvas.GetComponent<Canvas>().enabled = false;

    }

public void quitGame()
    {

        Application.Quit();
        Debug.Log("quit");

    }

public void team()
    {

        //Our facebook page/website
        Application.OpenURL("www.facebook.com/harvesthandsgame");

    }

void Start()
    {

        resolutions = Screen.resolutions;

        //resolutionDrop.options.Clear();

        for(int i = 0; i < resolutions.Length; i++)
        {
            resolutionDrop.options[i].text = (resolutions[i]).ToString();
            resolutionDrop.value = i;
        }

    }

public void qualityUp()
    {

        QualitySettings.IncreaseLevel(true);

    }

public void qualityDown()
    {

        QualitySettings.DecreaseLevel(true);

    }

public void quality()
    {

        if (qualityDrop.value == 0)
        {

            QualitySettings.SetQualityLevel(0,true);

        }

        if (qualityDrop.value == 1)
        {

            QualitySettings.SetQualityLevel(1,true);

        }

        if (qualityDrop.value == 2)
        {

            QualitySettings.SetQualityLevel(2,true);

        }

        if (qualityDrop.value == 3)
        {

            QualitySettings.SetQualityLevel(3,true);

        }

        if (qualityDrop.value == 4)
        {

            QualitySettings.SetQualityLevel(4,true);

        }

        if (qualityDrop.value == 5)
        {

            QualitySettings.SetQualityLevel(5,true);

        }

    }
}
