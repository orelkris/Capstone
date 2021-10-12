using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System;

public class PlayerManager : MonoBehaviour
{
    private Player pPlayer;
    private string playerType;

    private PhotonView PV;
    private AudioBehaviour AB;

    public InputActionReference pttReference;
    public PlayerInput playerInput;
    public UnityEvent onMicToggle;


    public string PlayerType { get => playerType; }
    public Player PPlayer { get => pPlayer; }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        AB = GetComponentInChildren<AudioBehaviour>();

        // Set network values
        pPlayer = PhotonNetwork.LocalPlayer;
        playerType = (string)pPlayer.CustomProperties["class"];

        // Destroy what we dont need
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pttReference.action.started += ActivateMic;
        pttReference.action.canceled += DeactivateMic;
/*        pttReference.action.performed += ToggleMic;
        pttReference.action.performed -= ToggleMic;*/
    }
/*    private void ToggleMic(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            Debug.Log("mic activated");
        }
        else if (obj.canceled)
        {
            Debug.Log("mic deactivated");
        }
    }*/
    
    private void ActivateMic(InputAction.CallbackContext obj)
    {
        Debug.Log("mic activated");
        AB.MicOn();
    }

    private void DeactivateMic(InputAction.CallbackContext obj)
    {
        Debug.Log("mic deactivated");
        AB.MicOff();
    }

  
}