using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerManager : MonoBehaviourPun
{
    private Player player;
    private string playerType;

    private AudioBehaviour AB;

    [SerializeField]
    private InputActionReference pttReference;

    public string PlayerType { get => playerType; }
    public Player Player { get => player; }

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
        } 

        if(player.CustomProperties["class"].Equals("hacker"))
        {
            player.TagObject = "Hacker";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AB = GetComponentInChildren<AudioBehaviour>();

        // Event handling
        pttReference.action.started += ActivateMic;
        pttReference.action.canceled += DeactivateMic;
    }

    // Mic Input
    private void ActivateMic(InputAction.CallbackContext obj) => AB.MicOn();
    private void DeactivateMic(InputAction.CallbackContext obj) => AB.MicOff();
}