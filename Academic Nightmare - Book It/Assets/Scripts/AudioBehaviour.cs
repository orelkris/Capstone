using Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudioBehaviour : MonoBehaviour
{
    // 
    private float currentVolume;

    // Mic Input
    private float sensitivity = 200;
    private float loudness = 0;

    //Player stuff
    PhotonView PV;
    AudioSource _audio;
    SphereCollider sc;

    private void Awake()
    {

        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        sc = GetComponentInChildren<SphereCollider>();
        _audio = GetComponent<AudioSource>();

        _audio.clip = Microphone.Start(null, true, 10, 44100);
        _audio.loop = false;

        while (!(Microphone.GetPosition(null) > 0)) { }
        _audio.Play();
    }

    void Update()
    {
        if (!PV.IsMine) return;
        EmitAudio(6);
    }

    void EmitAudio(float thresh)
    {
        // Mic Input
        GameObject sphere = sc.gameObject;
        float scaling = GetAverageVolume(thresh);
        /*Debug.Log(GetAverageVolume());*/

        if (scaling == 3)
            sc.radius = 0;
        else
            sc.radius = scaling;
        Debug.Log(sc.radius);
    }

    // Mic Input
    float GetAverageVolume(float thresh)
    {
        float[] data = new float[256];
        float a = 0;

        _audio.GetOutputData(data, 0);
        foreach (var s in data)
        {
            a += Mathf.Abs(s);
        }

        float avgVolume = a / 256;

        if (avgVolume > 3)
        {
            return 3;
        }
        else if (avgVolume > 0.001)
        {
            return a + thresh;
        }
        else
        {
            return 0;
        }
    }
}
