using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public static NetworkController Instance;
    [SerializeField] TMP_InputField roomNameInputField;

    //bool to check if using for quick testing
    public bool isTest = false;
    //Room option to set the number of players to 2
    public byte MaxPlayers = 2;

    private void Awake()
    {
        Instance = this;
    }

    //Intiate connection to server
    private void Start()
    {
        isTest = false;
        Debug.Log("Connecting to Server");
        PhotonNetwork.ConnectUsingSettings();
    }

    //Get message back from connecting to server
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    //Joined a lobby
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("Joined Lobby");
    }

    //Create or join room for testing
    public void DevTestCreateJoinRoom()
    {
        isTest = true;
        Photon.Realtime.RoomOptions opts = new Photon.Realtime.RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, opts, Photon.Realtime.TypedLobby.Default);
    }

    //Failed to create a room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Room Failed to Create");
    }

    //Set the current player to be player one
    public void SetPlayerOne()
    {
        GameStateController.isPlayerOne = true;
    }

    //Set the current player to be player two
    public void SetPlayerTwo()
    {
        GameStateController.isPlayerOne = false;
    }

    //Joined Room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //If running test load level
        if (isTest)
        {
            PhotonNetwork.LoadLevel(1);
        }

    }
}
