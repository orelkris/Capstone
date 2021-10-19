using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AI : MonoBehaviourPun
{
    NavMeshAgent agent;
    Animator anim;
    private Transform player = null;
    public GameObject patrolPath = null; 
    public List<Transform> checkpoints = new List<Transform>();
    public enum DIFFICULTY
    {
        EASY, MEDIUM, HARD
    };

    State currentState;
    private PhotonView pv;
    public AudioSource footstep;

    // AI attributes
    public float suspicionRate;

    // AI vision
    public float visDist = 40.0f;
    public float visAngle = 130.0f;

    // AI chase
    public float movingSpeed;
    public float chaseDuration = 5f;

    // AI hearing
    public float soundDetected = 0;
    public Vector3 noisePosition;
    public float hearingRange = 75f;
    public float hearingSensitivity = 0.01f;
    public float spinSpeed = 3f;
    public bool canSpin = false;
    public float isSpinningTime;
    public float spinTime = 3f;

    // AI memory
    public bool aiMemorizePlayer = false;
    public float memoryStartTime = 10f;
    public float increasingMemoryTime;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        pv = photonView;
        suspicionRate = 0;
        footstep = gameObject.GetComponent<AudioSource>();

        if (GameStateController.isPlayerOne)
        {
            // create librarian
            this.tag = "Librarian";
            player = GameObject.Find("PlayerOne(Clone)").transform;
            patrolPath = GameObject.Find("Checkpoints-Librarian");
        }
        else
        {
            // create assistant
            tag = "Assistant";
            player = GameObject.Find("PlayerTwo(Clone)").transform;
            patrolPath = GameObject.Find("Checkpoints-Assistant");
        }
        foreach (Transform child in patrolPath.transform)
        {
            checkpoints.Add(child);
        }
        currentState = new Idle(this, agent, player, State.STATE.IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        // start updating when game is ready
        if (player)
        {
            Debug.Log(currentState.currentState);
            currentState = currentState.Process();
        }
    }

    [PunRPC]
    public void SetSoundDetected(float s)
    {
        //Debug.Log("Noise: " + s);
        soundDetected = s;
        noisePosition = player.transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide with " + collision.gameObject);
        if(collision.gameObject.name == "PlayerTwo(Clone)")
        {
            Debug.Log("Caught");
            // penalty here and respawn player
        }
    }

}
