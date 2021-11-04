using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private float startTime;
    private float timeLeft;
    private bool isPaused;
    private bool isDone;
    public bool initilzed;
    private PhotonView pv;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        initilzed = false;
        setTimer(300.0f);//Default Time is seconds
        startTimer();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !initilzed)
        {
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                pv.RPC("initTimer", RpcTarget.All, null);
            }
        }
        if (!isPaused)
            timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            isPaused = true;
            isDone = true;
            timeLeft = 0f;
        }
        //Debug.Log(PhotonNetwork.Time);
    }

    public void setTimer(float t)
    {
        isPaused = true;
        isDone = false;
        startTime = t;
        timeLeft = t;
    }

    [PunRPC]
    public void initTimer()
    {
        setTimer(480.0f);
        startTimer();
        initilzed = true;
        Debug.Log("Timer Initialized");
    }

    [PunRPC]
    public void startTimer()
    {
        isPaused = false;
    }

    [PunRPC]
    public void pauseTimer()
    {
        isPaused = true;
    }

    public bool isTimerPaused()
    {
        return isPaused;
    }
    
    public float getTimeLeft()
    {
        return timeLeft;
    }

    public float getTimeLeftPercentage()
    {
        return (timeLeft / startTime * 100);
    }

    public bool isTimerDone()
    {
        return isDone;
    }
}
