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
    public List<GameObject> checkpoints = new List<GameObject>();
    public enum DIFFICULTY
    {
        EASY, MEDIUM, HARD
    };

    State currentState;
    private PhotonView pv;
    public AudioSource footstep;

    // AI attributes
    //public int suspicionRate;


    // AI vision
    public float visDist = 20.0f;
    public float visAngle = 45.0f;

    // AI chase
    public float movingSpeed;
    public float chaseDuration = 5f;

    // AI hearing
    public float soundDetected = 0;
    public Vector3 noisePosition;
    public float hearingRange = 50f;
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
        //suspicionRate = 0;
        footstep = gameObject.GetComponent<AudioSource>();
        for (int i = 1; i < 24; i++)
        {
            checkpoints.Add(GameObject.Find("cp" + i));
        }

        if (GameStateController.isPlayerOne)
        {
            // create librarian
            player = GameObject.Find("PlayerOne(Clone)").transform;
        }
        else
        {
            // create assistant
            this.tag = "Assistant";
            player = GameObject.Find("PlayerTwo(Clone)").transform;
        }
        currentState = new Idle(this, agent, player);
    }

    // Update is called once per frame
    void Update()
    {
        // start updating when game is ready
        if (player)
        {
            currentState = currentState.Process();
        }
    }

    [PunRPC]
    public void SetSoundDetected(float s)
    {
        this.soundDetected = s;
        this.noisePosition = player.transform.position;
    }
}
