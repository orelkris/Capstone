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
    void Start()
    {
        Debug.Log("start voice");
        view = photonView;
        VoiceRecorder.TransmitEnabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(PushButton))
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
        }
    }
}
