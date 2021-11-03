using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using StarterAssets;

public class PlayerManager : MonoBehaviourPun
{
    // Player Info
    Player player;
    string playerType;
    public Player Player { get => player; }
    public string PlayerType { get => playerType; }

    // GameObject References
    [SerializeField] GameObject playerModel;
    private Camera mainCamera;

    // Input System
    [SerializeField] InputActionReference pttReference;

    private void Awake()
    {
        // Set network values
        player = PhotonNetwork.LocalPlayer;
        playerType = (string)player.CustomProperties["class"];

        // Destroy what we dont need
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
            Destroy(playerModel.GetComponentInChildren<FirstPersonController>());
            Destroy(playerModel.GetComponentInChildren<PlayerInput>());
            Destroy(playerModel.GetComponentInChildren<StarterAssetsInputs>());
        } 
    }

    void Start()
    {
        Debug.Log("PhotonView = " + photonView + "PlayerType = " + playerType);



        /*AB = GetComponentInChildren<AudioBehaviour>();*/

        // Event handling
        /*pttReference.action.started += ActivateMic;
        pttReference.action.canceled += DeactivateMic;*/
    }

    // Mic Input
/*    private void ActivateMic(InputAction.CallbackContext obj) => AB.MicOn();
    private void DeactivateMic(InputAction.CallbackContext obj) => AB.MicOff();*/

}