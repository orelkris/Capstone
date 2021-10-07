using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class Assistant : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;
    private Transform player;
    public static List<GameObject> checkpoints = new List<GameObject>();
    State currentState;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("PlayerTwo(Clone)").transform;
        currentState = new Idle(this.gameObject, agent, player);
        for(int i = 1; i < 24; i++)
        {
            checkpoints.Add(GameObject.Find("cp" + i));
        }
        Debug.Log(checkpoints.Count);
    }

    // Update is called once per frame
    void Update()
    {
        // ENTER UPDATE EXIT
        //add footstep sound
        currentState = currentState.Process();
    }
}