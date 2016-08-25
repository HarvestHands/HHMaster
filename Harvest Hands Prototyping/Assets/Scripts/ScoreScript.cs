using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScoreScript : NetworkBehaviour
{
    [SyncVar]
    public int score = 0;
    public int oldScore = 0;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    [Command]
    public void CmdAddScore(int _score)
    {
        oldScore = score;
        score += _score;
    }
}
