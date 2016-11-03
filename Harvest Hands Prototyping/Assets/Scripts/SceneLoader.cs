using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool loadScene = false;

    public int scene;
    public Canvas loadingCanvas;
    public float loadWaitTime = 5f;

	// Use this for initialization
	void Start ()
    {
        loadingCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void AsyncLoadScene(int sceneIndex)
    {
        scene = sceneIndex;
        if (loadingCanvas != null)
            loadingCanvas.enabled = true;


        StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene()
    {

        yield return new WaitForSeconds(loadWaitTime);

        //Start asynchronous operation to load new scene
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
            yield return null;
        Debug.Log("Async done");
    }

}
