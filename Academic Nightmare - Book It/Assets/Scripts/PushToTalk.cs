using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class PushToTalk : MonoBehaviourPun
{
    public KeyCode PushButton = KeyCode.L;
    public Recorder VoiceRecorder;
    private PhotonView view;

    public static float currentPeak = 0f;
    void Start()
    {
        Debug.Log("start voice");
        view = photonView;
        VoiceRecorder.TransmitEnabled = false;
    }

    void Update()
    {
        //currentPeak = VoiceRecorder.LevelMeter.CurrentPeakAmp;
        //Debug.Log("FINDING SECOND LEVEL " + VoiceRecorder.LevelMeter.CurrentAvgAmp);

        if (Input.GetKeyDown(PushButton))
        {
            Debug.Log("here");
            if(view.IsMine)
            {
                Debug.Log("Start talking");
                VoiceRecorder.TransmitEnabled = true;
            }

        }
        else if(Input.GetKeyUp(PushButton))
        {
            if (view.IsMine)
            {
                Debug.Log("Stop talking");
                VoiceRecorder.TransmitEnabled = false;
            }

            currentPeak = 0;
        }

        if(VoiceRecorder.TransmitEnabled)
        {
            currentPeak = VoiceRecorder.LevelMeter.CurrentPeakAmp;
        }
        else
        {
            currentPeak = 0;
        }

        Debug.Log("FINDING LEVEL " + VoiceRecorder.LevelMeter.CurrentPeakAmp);
        
    }
}
