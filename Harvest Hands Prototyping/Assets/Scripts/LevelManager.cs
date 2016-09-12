using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance = null;
    public bool LoadedLevel = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

	public void SetLoadedLevelTrue()
    {
        LoadedLevel = true;
    }

    public void SetLoadedLevelFalse()
    {
        LoadedLevel = false;
    }

}
