using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class PlayerManager : MonoBehaviourPun
{
    private Player player;
    private string playerType = null;

    private AudioBehaviour AB;

    [SerializeField] InputActionReference pttReference;

    public string PlayerType { get => playerType; }
    public Player Player { get => player; }

    private void Awake()
    {
        // Set network values
        player = PhotonNetwork.LocalPlayer;
        playerType = (string)player.CustomProperties["class"];
        /*playerType = player.;*/

        // Destroy what we dont need
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
        } 

        /*if(playerType.Equals("hacker"))
        {
            player.TagObject = "Hacker";
        }
        else
        {
            player.TagObject = "Thief";
        }*/
    }

    // Start is called before the first frame update
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