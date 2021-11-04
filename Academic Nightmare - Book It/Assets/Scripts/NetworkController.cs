using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public static NetworkController Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField devroomNameInputField;
    [SerializeField] TMP_Dropdown sceneSelectDropdown;
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject playerText;
    [SerializeField] GameObject playerSelectDropdown;
    [SerializeField] GameObject levelText;
    [SerializeField] GameObject levelSelectDropdown;
    [SerializeField] GameObject startGameButton;

    public bool isDevBuild = false;
    //bool to check if using for quick testing
    public bool isTest = false;
    //Room option to set the number of players to 2
    public byte MaxPlayers = 2;
    //For first menu boot up to go to dev menu
    private bool devStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    //Intiate connection to server
    private void Start()
    {
        Debug.Log("Connecting to Server");
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    //Get message back from connecting to server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    //Joined a lobby
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("Joined Lobby");
        if (GameStateController.isDevBuild && !devStarted)
        {
            MenuManager.Instance.OpenMenu("DevTestMenu");
            devStarted = true;
        }
        else
        {
            MenuManager.Instance.OpenMenu("TitleMenu");
        }

        ChangeName();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        RoomOptions opts = new RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.CreateRoom(roomNameInputField.text, opts, TypedLobby.Default);
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    //Create or join room for testing
    public void DevTestCreateJoinRoom()
    {
        isTest = true;
        RoomOptions opts = new RoomOptions();
        opts.IsOpen = true;
        opts.IsVisible = true;
        opts.MaxPlayers = MaxPlayers;

        PhotonNetwork.JoinOrCreateRoom(devroomNameInputField.text, opts, TypedLobby.Default);
    }

    //Failed to create a room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Room Failed to Create");
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    //Set the current player to be player one
    public void SetPlayerOne()
    {
        GameStateController.isPlayerOne = true;

        // Test
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("class"))
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("class");
        PhotonNetwork.LocalPlayer.CustomProperties.Add("class", "hacker");
    }

    //Set the current player to be player two
    public void SetPlayerTwo()
    {
        GameStateController.isPlayerOne = false;

        // Test
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("class"))
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("class");
        PhotonNetwork.LocalPlayer.CustomProperties.Add("class", "thief"); // Identifies player type by custom props
    }

    public void SetGhost()
    {
        GameStateController.isGhost = false;

        // Test
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("class"))
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("class");
        PhotonNetwork.LocalPlayer.CustomProperties.Add("class", "ghost");
    }

    //Joined Room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //If running test load level
        if (isTest)
        {
            //Load Scene based on Build Settings
            //Keep Dropdown order same as scene build order with Main Menu scene being scene 0
            PhotonNetwork.LoadLevel(sceneSelectDropdown.value + 1);
        }
        else
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Length; i++)
            {
                Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            SetActiveRoomItems();
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPC_GetPlayer", RpcTarget.Others, null);
                photonView.RPC("RPC_GetLevelSelect", RpcTarget.Others, null);
            }
            else
            {
                SetPlayerOne();
            }

            MenuManager.Instance.OpenMenu("RoomMenu");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        SetActiveRoomItems();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        MenuManager.Instance.OpenMenu("TitleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void ChangeName()
    {
        if (string.IsNullOrEmpty(playerNameInputField.text))
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 9999).ToString("0000");
            playerNameInputField.text = PhotonNetwork.NickName;
        }
        else
        {
            PhotonNetwork.NickName = playerNameInputField.text;
        }
    }

    //Set the room item to appear or not based on if client is master or not
    void SetActiveRoomItems()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        playerSelectDropdown.SetActive(PhotonNetwork.IsMasterClient);
        playerText.SetActive(!PhotonNetwork.IsMasterClient);
        levelSelectDropdown.SetActive(PhotonNetwork.IsMasterClient);
        levelText.SetActive(!PhotonNetwork.IsMasterClient);
    }

    public void PlayerChanged()
    {
        if (playerSelectDropdown.GetComponent<TMP_Dropdown>().value == 0)
            SetPlayerOne();
        else
            SetPlayerTwo();

        photonView.RPC("RPC_SetPlayer", RpcTarget.Others, !GameStateController.isPlayerOne);
    }

    [PunRPC]
    void RPC_GetPlayer()
    {
        photonView.RPC("RPC_SetPlayer", RpcTarget.Others, !GameStateController.isPlayerOne);
    }

    [PunRPC]
    void RPC_SetPlayer(bool isPlayerOne)
    {
        if (isPlayerOne)
            SetPlayerOne();
        else
            SetPlayerTwo();
        playerText.GetComponent<TMP_Text>().text = "Player " + (GameStateController.isPlayerOne ? "1" : "2");
        playerSelectDropdown.GetComponent<TMP_Dropdown>().value = (GameStateController.isPlayerOne ? 0 : 1);
    }

    public void LevelChanged()
    {
        GameStateController.levelSelect = levelSelectDropdown.GetComponent<TMP_Dropdown>().value + 1;

        photonView.RPC("RPC_SetLevelSelect", RpcTarget.Others, GameStateController.levelSelect);
    }

    [PunRPC]
    void RPC_GetLevelSelect()
    {
        photonView.RPC("RPC_SetLevelSelect", RpcTarget.Others, GameStateController.levelSelect);
    }

    [PunRPC]
    void RPC_SetLevelSelect(int level)
    {
        GameStateController.levelSelect = level;
        levelSelectDropdown.GetComponent<TMP_Dropdown>().value = (GameStateController.levelSelect - 1);
        if (GameStateController.levelSelect == 1)
            levelText.GetComponent<TMP_Text>().text = "Level";
        else if(GameStateController.levelSelect == 2)
            levelText.GetComponent<TMP_Text>().text = "Sample";
        else
            levelText.GetComponent<TMP_Text>().text = "Unknown";
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(GameStateController.levelSelect);
    }
}
