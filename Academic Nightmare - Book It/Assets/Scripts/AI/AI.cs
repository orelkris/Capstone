using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
         currentState = new Idle(this.gameObject, agent, player);
    }

    // Update is called once per frame
    void Update()
    {
        // ENTER UPDATE EXIT
        //add footstep sound
        //Debug.Log("Current State: " + currentState);
        currentState = currentState.Process();
    }
}