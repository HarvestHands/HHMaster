using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class splash : MonoBehaviour {

	public Image splashImage;

	private float timeToFade = 0;

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene(1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeToFade += Time.deltaTime;

		if (timeToFade >= 2.0f)
			splashImage.CrossFadeAlpha (0f, 0.8f, false);	
	}
}