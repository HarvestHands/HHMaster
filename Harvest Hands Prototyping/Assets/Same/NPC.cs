using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    NavMeshAgent agent;

    public float minWaitTime = 4f;
    public float maxWaitTime = 20f;
    float timeToNewAction = 0;
    public PathManager.GoalPlace goalArea = PathManager.GoalPlace.Market;
    public List<Transform> path;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new List<Transform>();
        //Invoke("LateStart", 0.5f);
    }

    void LateStart()
    {
        //path = PathManager.instance.GetNewPath(ref goalArea);
        //agent.SetDestination(path[0].position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (path.Count == 0)
        {
            timeToNewAction -= Time.deltaTime;

            if (timeToNewAction < 0)
            {
                path = PathManager.instance.GetNewPath(ref goalArea);
                agent.SetDestination(path[0].position);
            }
        }

        if (path.Count > 0)
        {
            if (Vector3.Distance(transform.position, path[0].position) <= agent.stoppingDistance)
            {
                if (path.Count > 1)
                {
                    path.RemoveAt(0);
                    agent.SetDestination(path[0].position);
                }
                else
                {
                    transform.rotation = path[0].rotation;
                    path.RemoveAt(0);
                    timeToNewAction = Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }


	}
}
