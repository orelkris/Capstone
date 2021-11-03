using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class AI : MonoBehaviourPun
{
    NavMeshAgent agent;
    Animator anim;
    private GameObject player = null;
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
    public float visDist = 60.0f;
    public float visAngle = 130.0f;

    // AI chase
    public float movingSpeed;
    public float chaseDuration = 5f;

    // AI hearing
    public float soundDetected = 0;
    public Vector3 noisePosition;
    public float hearingRange = 100f;
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

        // tag not created on the rigt cycle
        if (GameStateController.isPlayerOne)
        {
            // create librarian
            tag = "Librarian";
            player = GameObject.FindGameObjectWithTag("Hacker");
            patrolPath = GameObject.Find("Checkpoints-Librarian");
        }
        else
        {
            // create assistant
            tag = "Assistant";
            player = GameObject.FindGameObjectWithTag("Thief");
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
        if (GameStateController.isPlayerOne)
        {
            // create librarian
            tag = "Librarian";
            player = GameObject.FindGameObjectWithTag("Hacker");
            patrolPath = GameObject.Find("Checkpoints-Librarian");
        }
        else
        {
            // create assistant
            tag = "Assistant";
            player = GameObject.FindGameObjectWithTag("Thief");
            patrolPath = GameObject.Find("Checkpoints-Assistant");
        }
        if (player)
        {
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

    [PunRPC]
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            Debug.Log("Reset Floor");

            PhotonNetwork.Destroy(player.transform.parent.GetComponent<PhotonView>());
            PhotonNetwork.Destroy(this.transform.parent.GetComponent<PhotonView>());

            RoomManager.Instance.SpawnPlayer();
            // TODO: reflect on clock
        }
    }

}
