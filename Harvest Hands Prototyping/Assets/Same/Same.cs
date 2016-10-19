using UnityEngine;
using System.Collections;

public class Same : MonoBehaviour
{
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            agent.SetDestination(player.transform.position);
	}
}
