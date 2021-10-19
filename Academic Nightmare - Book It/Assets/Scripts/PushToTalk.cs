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
    public float sensitivity = 200;
    public float loudness;
    AudioClip _clipRecord = null;
    private int _window = 256;
    private string _device;
    AudioSource _audio;
    private bool _isInitialized = false;
    private float volume;
    private AI enemy;
    private Vector3 noisePosition;
    public float spinSpeed = 3f;

    public static float currentPeak = 0f;

    private PhotonView view;
    void Start()
    {
        Debug.Log("start voice");
        view = photonView;
        VoiceRecorder.TransmitEnabled = false;
        _device = Microphone.devices[0];
        if (!GameStateController.isPlayerOne)
        {
            //enemy = GameObject.FindGameObjectWithTag("Assistant").GetComponent<AI>();
            enemy = FindObjectOfType<AI>();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(PushButton))
        {
            if(view.IsMine)
            {
                Debug.Log("Start talking");
                VoiceRecorder.TransmitEnabled = true;
                InitMic();
                _isInitialized = true;
            }
        }
        else if(Input.GetKeyUp(PushButton))
        {
            if (view.IsMine)
            {
                Debug.Log("Stop talking");
                VoiceRecorder.TransmitEnabled = false;
                StopMicrophone();
                _isInitialized = false;
            }
        }
        loudness = LevelMax();
        if(loudness > 0)
        {
            //if(view.IsMine) view.RPC("SensingSound", RpcTarget.Others, loudness);
            enemy.SetSoundDetected(loudness);
        }

        if(VoiceRecorder.TransmitEnabled)
        {
            currentPeak = loudness;
        }
        else
        {
            currentPeak = 0f;
        }

    }

    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_window];
        int micPosition = Microphone.GetPosition(null) - (_window + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        if (_clipRecord)
        {
            _clipRecord.GetData(waveData, micPosition);
            // Getting a peak on the last 128 samples
            for (int i = 0; i < _window; i++)
            {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak)
                {
                    levelMax = wavePeak;
                }
            }
        }
        
        return levelMax;
    }

    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }

}
