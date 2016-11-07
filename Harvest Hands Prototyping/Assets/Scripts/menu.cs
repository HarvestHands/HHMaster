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
    public Text qualityLevel;
    private int currentQuality;

public void Update()
    {

        currentQuality = QualitySettings.GetQualityLevel();

        if(currentQuality == 0)
        {

            qualityLevel.text = "Lowest";

        }

        if(currentQuality == 1)
        {

            qualityLevel.text = "Low";

        }

        if(currentQuality == 2)
        {

            qualityLevel.text = "Medium";

        }

        if(currentQuality == 3)
        {

            qualityLevel.text = "Good";

        }

        if(currentQuality == 4)
        {

            qualityLevel.text = "Great";

        }

        if(currentQuality == 5)
        {

            qualityLevel.text = "Awesome";

        }
    }

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
