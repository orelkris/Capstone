using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, ROAM, PURSUE, SUSPECT
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE currentState;
    protected EVENT stage;
    protected AI npc;
    protected GameObject player;
    public STATE prevState;
    protected State nextState;
    protected NavMeshAgent agent;
    //public MonoBehaviour mb;

    public State(AI _npc, NavMeshAgent _agent, GameObject _player, STATE _prevState)
    {
        npc = _npc;
        agent = _agent;
        stage = EVENT.ENTER;
        player = _player;
        //mb = GameObject.FindObjectOfType<MonoBehaviour>();
    }

    //public State()
    //{
        
    //}
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
        Vector3 direction = player.transform.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        Vector3 eyesPos = new Vector3(agent.transform.position.x, agent.height, agent.transform.position.z);
        float sphereCastRadius = 4.0f;
        RaycastHit hit;
        if (direction.magnitude < npc.visDist && angle < npc.visAngle)
        {
            //Debug.Log("Object detected between AI and player at"+ hit.distance);
            //Debug.DrawRay(eyesPos, direction, Color.red, npc.visDist);
            if (Physics.SphereCast(eyesPos, sphereCastRadius, direction, out hit, npc.visDist))
            {
                if (hit.transform.gameObject == player)
                {
                    Debug.Log("Player is seen");
                    return true;
                }
            }
        }
        Debug.Log("Player not seen");
        return false;
    }

    public bool CheckSoundDetected()
    {
        if(npc.soundDetected > npc.hearingSensitivity)
        {
            float noiseDistance = Vector3.Distance(npc.transform.position, npc.noisePosition);
            if (noiseDistance < npc.hearingRange)
            {
                Debug.Log("Noise heard");
                return true;
            }
        }
        return false;
    }
}

public class Idle : State
{
    public Idle(AI _npc, NavMeshAgent _agent, GameObject _player, STATE _prevState)
        : base(_npc, _agent, _player, _prevState)
    {
        currentState = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        // Check if both players are connected and game has started. replace with global variable if any
        if (player)
        {
            if (CanSeePlayer() == true)
            {
                nextState = new Pursue(npc, agent, player, currentState);
                stage = EVENT.EXIT;
            }
            else if (CheckSoundDetected() == true)
            {
                nextState = new Suspect(npc, agent, player, currentState);
                stage = EVENT.EXIT;
            }
            //else if (prevState == STATE.PURSUE || prevState == STATE.SUSPECT)
            //{
            //    nextState = new Idle(npc, agent, player, currentState);
            //    stage = EVENT.EXIT;
            //}
            else
            {
                nextState = new Roam(npc, agent, player, currentState);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Roam : State
{
    int currentIndex = -1;
    GameObject assignedCheckpoint = null;
    public Roam(AI _npc, NavMeshAgent _agent, GameObject _player, STATE _prevState)
        : base(_npc, _agent, _player, _prevState)
    {
        currentState = STATE.ROAM;
        agent.speed = 3.5f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < npc.checkpoints.Count; i++)
        {
            Transform thisWP = npc.checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.position);
            if (distance < lastDist)
            {
                currentIndex = i;
                lastDist = distance;
            }
        }
        base.Enter();
    }

    public override void Update()
    {
        //TODO: randomize the pattern by choosing from 1 of the 3 closest checkpoint
        if (agent.remainingDistance < 1)
        {
            //reach to the end of the checkpoint list
            if (currentIndex >= npc.checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            //move to next check point
            agent.SetDestination(npc.checkpoints[currentIndex].transform.position);
            Debug.Log("Current Index: " + currentIndex);
        }
        //start chasing player if it sees one
        if (CanSeePlayer() == true)
        {
            nextState = new Pursue(npc, agent, player, currentState);
            stage = EVENT.EXIT;
        }
        // if AI hear sounds, start suspecting and think about what to do
        else if(CheckSoundDetected() == true)
        {
            nextState = new Suspect(npc, agent, player, currentState);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pursue : State
{
    float pursueTime = 5f;
    public Pursue(AI _npc, NavMeshAgent _agent, GameObject _player, STATE _prevState)
        : base(_npc, _agent, _player, _prevState)
    {
        currentState = STATE.PURSUE;
        agent.speed = 4.8f;
        agent.isStopped = false;
        pursueTime = Time.deltaTime;
    }

    public override void Enter()
    {
        Debug.Log("Enter Pursue");
        base.Enter();
    }

    public override void Update()
    {
        //If enemy cannot see player, start counting down
        if (agent.hasPath)
        {
            agent.SetDestination(player.transform.position);
            if (CanSeePlayer())
            {
                Debug.Log("Reset to 5");
                //npc.chaseDuration = 5f;
                pursueTime = 5f;
            }
            else
            {
                //npc.chaseDuration -= Time.deltaTime;
                //if (npc.chaseDuration <= 0)
                pursueTime -= Time.deltaTime;
                if(pursueTime <= 0)
                {
                    agent.SetDestination(agent.transform.position);
                    nextState = new Roam(npc, agent, player, currentState);
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

public class Suspect : State
{
    // this state should take action base on suspect rate
    public Suspect(AI _npc, NavMeshAgent _agent, GameObject _player, STATE _prevState)
    : base(_npc, _agent, _player, _prevState)
    {
        currentState = STATE.SUSPECT;
        agent.speed = 6f;
        npc.suspicionRate = 10;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(CheckSoundDetected() == true && CanSeePlayer() == false)
        {
            agent.SetDestination(npc.noisePosition);
            npc.soundDetected = 0;
            nextState = new Idle(npc, agent, player, currentState);
            stage = EVENT.EXIT;
        }
        else if(CanSeePlayer() == true)
        {
            nextState = new Pursue(npc, agent, player, currentState);
            stage = EVENT.EXIT;
        }
        else
        {
            nextState = new Roam(npc, agent, player, currentState);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}