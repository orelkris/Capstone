using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, ROAM, PURSUE, STACK, ATTACK, SLEEP
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Transform player;
    protected State nextState; // not the enum state
    protected NavMeshAgent agent;

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    // progresses the state through each of the stages
    public State Process()
    {
        if (stage == EVENT.ENTER)
        {
            Enter();
        }

        if (stage == EVENT.UPDATE)
        {
            Update();
        }

        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    //TODO: add raycast so player can hide behind object
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        float maxRange = 15f;
        RaycastHit hit;
        if (direction.magnitude < visDist && angle < visAngle)
        {
            //Debug.Log("Object detected between AI and player at"+ hit.distance);
            if (Physics.Raycast(agent.transform.position, (player.position - agent.transform.position), out hit, maxRange))
            {
                if (hit.transform == player)
                {
                    Debug.Log("Player is seen");
                }

            }
            return true;
        }

        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }

        return false;
    }

}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        // 10% of the time...go to next stage
        else if (Random.Range(0, 100) < 30)
        {
            nextState = new Roam(npc, agent, player);
            stage = EVENT.EXIT;
        }
        // base.Update();
    }

    public override void Exit()
    {
        // clean up animation
        base.Exit();
    }
}

// ROAM == PATROL
public class Roam : State
{
    int currentIndex = -1;
    GameObject assignedCheckpoint = null;
    public Roam(GameObject _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.ROAM;
        agent.speed = 2f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //Why compare to infinity?
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }

        base.Enter();
    }

    public override void Update()
    {
        //TODO: can we randomize the patrol pattern?
        if (agent.remainingDistance < 1)
        {
            //reach to the end of the checkpoint list
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            //move to next check point
            //add rotation
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
        //start chasing player if it sees one
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        // base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pursue : State
{
    //float pursueTime = 0f;
    float pursueTime = 5f;
    public Pursue(GameObject _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
        pursueTime = Time.deltaTime;
    }

    public override void Enter()
    {
        base.Enter();
    }

    //TODO: Change the stop chasing condition to increase difficulty
    public override void Update()
    {
        //pursueTime += Time.deltaTime;
        //Debug.Log("Pursue for: " + pursueTime);
        //agent.SetDestination(player.position);
        //if (agent.hasPath)
        //{
        //Return to closest patrol point instead
        //    if(!CanSeePlayer() && pursueTime > 10)
        //    {
        //        nextState = new Roam(npc, agent,  player);
        //        stage = EVENT.EXIT;
        //        Debug.Log("Stop Chasing");
        //    }
        //}


        //Solution 2: Player has to be out of sight for 5 seconds to trigger guard to leave
        //If enemy cannot see player, start counting down
        if (agent.hasPath)
        {
            agent.SetDestination(player.position);
            Debug.Log("Purse time left: " + pursueTime);
            if (CanSeePlayer())
            {
                pursueTime = 5f;
            }
            else
            {
                pursueTime -= Time.deltaTime;
                if (pursueTime <= 0)
                {
                    agent.SetDestination(agent.transform.position);
                    nextState = new Roam(npc, agent, player);
                    stage = EVENT.EXIT;
                    Debug.Log("Stop Chasing");
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}