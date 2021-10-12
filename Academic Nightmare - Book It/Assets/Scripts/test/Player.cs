using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using StarterAssets;

public class Player : MonoBehaviour
{
    private Photon.Realtime.Player a;
    private bool isPlayerOne;

    private PhotonView PV;
    private AudioBehaviour AB;

    public bool IsPlayerOne { get => isPlayerOne; set { isPlayerOne = value; } }

    private void Awake()
    {
        a = PhotonNetwork.LocalPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
        }
    }
}