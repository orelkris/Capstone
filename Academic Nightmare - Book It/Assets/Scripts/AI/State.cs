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

    public STATE name;
    protected EVENT stage;
    protected AI npc;
    protected Transform player;
    protected STATE prevState;
    protected State nextState; // not the enum state
    protected NavMeshAgent agent;
    private MonoBehaviour mb;


    public State(AI _npc, NavMeshAgent _agent, Transform _player)
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
        RaycastHit hit;
        if (direction.magnitude < npc.visDist && angle < npc.visAngle)
        {
            //Debug.Log("Object detected between AI and player at"+ hit.distance);
            if (Physics.Raycast(agent.transform.position, (player.position - agent.transform.position), out hit, npc.visDist))
            {
                if (hit.transform == player)
                {
                    Debug.Log("Player is seen");
                    return true;
                }

            }
            //return true;
        }
        Debug.Log("Player not seen");
        return false;
    }

    public bool CheckSoundDetected()
    {
        if(npc.soundDetected > 0)
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

    public void StartCoroutine()
    {
        mb = GameObject.FindObjectOfType<MonoBehaviour>();
        if (mb != null)
        {
            mb.StartCoroutine(WaitForXSeconds(5));
        }
        else
        {
            Debug.Log("No MonoBehaviour object was found");
        }
            
    }

    private IEnumerator WaitForXSeconds(int x)
    {
        Debug.Log("Hi");
        yield return new WaitForSeconds(x);
    }

}

public class Idle : State
{
    public Idle(AI _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        //Debug.Log("Enter Idle");
        StartCoroutine();
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Update Idle");
        if (CanSeePlayer() == true)
        {
            //Debug.Log("Player spotted");
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        } else if(CheckSoundDetected() == true)
        {
            //Debug.Log("Noise heard");
            nextState = new Suspect(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 30)
        {
            //Debug.Log("Patrol");
            nextState = new Roam(npc, agent, player);
            stage = EVENT.EXIT;
        }
        // base.Update();
    }

    public override void Exit()
    {
        //Debug.Log("Exit Idle");
        // clean up animation
        prevState = STATE.IDLE;
        base.Exit();
    }
}

public class Roam : State
{
    int currentIndex = -1;
    GameObject assignedCheckpoint = null;
    public Roam(AI _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.ROAM;
        agent.speed = 3.5f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        //Debug.Log("Enter Roam");
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < npc.checkpoints.Count; i++)
        {
            GameObject thisWP = npc.checkpoints[i];
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
        //Debug.Log("Update Roam");
        //TODO: can we randomize the patrol pattern?
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
            //TODO: add rotation
            agent.SetDestination(npc.checkpoints[currentIndex].transform.position);
        }
        //start chasing player if it sees one
        if (CanSeePlayer() == true)
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        // if AI hear sounds, start suspecting and think about what to do
        else if(CheckSoundDetected() == true)
        {
            nextState = new Suspect(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exit Roam");
        prevState = STATE.ROAM;
        base.Exit();
    }
}

public class Pursue : State
{
    float pursueTime = 5f;
    public Pursue(AI _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.PURSUE;
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
            agent.SetDestination(player.position);
            Debug.Log("Purse time left: " + pursueTime);
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
                    nextState = new Roam(npc, agent, player);
                    stage = EVENT.EXIT;
                    Debug.Log("Stop Chasing");
                }
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exit Pursue");
        prevState = STATE.PURSUE;
        base.Exit();
    }
}

public class Suspect : State
{
    public Suspect(AI _npc, NavMeshAgent _agent, Transform _player)
    : base(_npc, _agent, _player)
    {
        name = STATE.SUSPECT;
        agent.speed = 6f;
    }

    public override void Enter()
    {
        //Debug.Log("Enter Suspect");
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("Update Suspect");
        if(CheckSoundDetected() == true && CanSeePlayer() == false)
        {
            agent.SetDestination(npc.noisePosition);
            npc.soundDetected = 0;
            nextState = new Idle(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else if(CanSeePlayer() == true)
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exit Suspect");
        prevState = STATE.SUSPECT;
        base.Exit();
    }
}