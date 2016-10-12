using UnityEngine;
using System.Collections;

public class AnimScript : MonoBehaviour
{
    [SerializeField]Animator anim;
    [SerializeField]float runSpeed;

    enum stateMachine
    {
        noState = -1,
        idle,
        walk,
        run
    };

    stateMachine sMach;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        anim.SetBool("IsWalking", true); //changes based ons true or false
        anim.SetFloat("New Float", runSpeed); //changes when over or under a float value
        sMach = 0;
        anim.SetInteger("State", (int)sMach);//changes based on state
        anim.Play("WalkCycle");//just plays animation


        

	}
}
