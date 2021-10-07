using System.IO;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;

    public GameObject player1SpawnPosition;
    public GameObject player2SpawnPosition;

    void Awake()
    {
        PV = GetComponent<PhotonView>();

        player1SpawnPosition = GameObject.Find("Player1SpawnPosition");
        player2SpawnPosition = GameObject.Find("Player2SpawnPosition");
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        /*    if(GameStateController.isPlayerOne)
              PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), new Vector3(0, 2, 0), Quaternion.identity);*/

        if (GameStateController.isPlayerOne)
        {
            PhotonNetwork.Instantiate("Player", player1SpawnPosition.transform.position, Quaternion.identity);

            // Hide the player 2 canvas object from player 1
            GameObject.Find("PanelCode").SetActive(false);
            GameObject.Find("Crosshair").SetActive(false);
        }
        else
        {
            PhotonNetwork.Instantiate("Player", player2SpawnPosition.transform.position, Quaternion.identity);
        }
    }
}
