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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        setTimer(300.0f);//Default Time is seconds
        startTimer();//Remove after Integration
    }

    void Update()
    {
        if (!isPaused)
            timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            isPaused = true;
            isDone = true;
            timeLeft = 0f;
        }
    }

    public void setTimer(float t)
    {
        isPaused = true;
        isDone = false;
        startTime = t;
        timeLeft = t;
    }

    public void startTimer()
    {
        isPaused = false;
    }

    public void pauseTimer()
    {
        isPaused = true;
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
