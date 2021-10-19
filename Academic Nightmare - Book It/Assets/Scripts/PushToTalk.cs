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
    private GameObject enemy;
    private AI enemyAI;
    private Vector3 noisePosition;
    public float spinSpeed = 3f;

    private PhotonView view;
    void Start()
    {
        Debug.Log("start voice");
        view = photonView;
        VoiceRecorder.TransmitEnabled = false;
        _device = Microphone.devices[0];
        if (GameStateController.isPlayerOne)
        {
            enemy = GameObject.Find("Librarian");
        }
        else
        {
            enemy = GameObject.Find("Assistant");
        }
        enemyAI = enemy.transform.GetChild(0).gameObject.GetComponent<AI>();    
    }

    void Update()
    {
        // push to talk
        if (Input.GetKeyDown(PushButton))
        {
            if (view.IsMine)
            {
                Debug.Log("Start talking");
                VoiceRecorder.TransmitEnabled = true;
                InitMic();
                _isInitialized = true;
            }
        }
        else if (Input.GetKeyUp(PushButton))
        {
            if (view.IsMine)
            {
                Debug.Log("Stop talking");
                VoiceRecorder.TransmitEnabled = false;
                StopMicrophone();
                _isInitialized = false;
            }
        }

        // toggle mic
        //if(Input.GetKeyDown(PushButton))
        //{
        //    if(view.IsMine)
        //    {
        //        VoiceRecorder.TransmitEnabled = !VoiceRecorder.TransmitEnabled;
        //        if(!VoiceRecorder.TransmitEnabled)
        //        {
        //            InitMic();
        //            _isInitialized = true;
        //            Debug.Log("Mic On");
        //        }
        //        else
        //        {
        //            StopMicrophone();
        //            _isInitialized = false;
        //            Debug.Log("Mic off");
        //        }
        //    }
        //}
        loudness = LevelMax();
        if(loudness > 0)
        {
            // if sound detected through microphone, send amplitude value to AI and the other player
            enemy.GetComponentInChildren<AI>().SetSoundDetected(loudness);
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
