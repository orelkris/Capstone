using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private PhotonView PV;

    public GameObject player1SpawnPosition;
    public GameObject player2SpawnPosition;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        player1SpawnPosition = GameObject.Find("Player1SpawnPosition");
        player2SpawnPosition = GameObject.Find("Player2SpawnPosition");

        if (PV.IsMine)
        {
            CreatePlayer();
        }

        // go back to menu scene if not connected
        /*if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);

            return;
        }*/
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        /*if (scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }*/
    }

    void CreatePlayer()
    {
        Debug.Log("player is " + PhotonNetwork.LocalPlayer.CustomProperties["class"]);

        if (PhotonNetwork.LocalPlayer.CustomProperties["class"].Equals("hacker"))
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), player1SpawnPosition.transform.position, Quaternion.identity);

            // Hide the player 2 canvas object from player 1
            GameObject.Find("PanelCode").SetActive(false);
            GameObject.Find("Crosshair").SetActive(false);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), player2SpawnPosition.transform.position, Quaternion.identity);
        }
    }

    /*#region PunCallBacks
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }
    #endregion*/
}
