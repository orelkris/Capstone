using Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudioBehaviour : MonoBehaviour
{
    // Some values
    private float currentVolume;
    private bool micHot = false;

    // Mic Settings
    public float threshold;
    private float sensitivity = 200;
    private float loudness = 0;
    
    //Player stuff
    PhotonView PV;
    AudioSource _audio;
    SphereCollider sc;

    public bool MicHot { get => micHot; }
    public float CurrentVolume { get => currentVolume; set => currentVolume = value; }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        sc = GetComponentInChildren<SphereCollider>();
        _audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;

        if(micHot) EmitAudio(6.0f);
    }

    public void MicOn()
    {
        _audio.clip = Microphone.Start(null, true, 100, 44100);
        _audio.loop = false;

        while (!(Microphone.GetPosition(null) > 0)) { }
        micHot = true;

        _audio.Play();
    }

    public void MicOff()
    {
        _audio.clip.UnloadAudioData();
        _audio.Stop();

        micHot = false;
    }

    void EmitAudio(float thresh)
    {
        // Mic Input
        float scaling = GetAverageVolume(thresh);
        /*Debug.Log(GetAverageVolume());*/

        if (scaling == thresh)
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
