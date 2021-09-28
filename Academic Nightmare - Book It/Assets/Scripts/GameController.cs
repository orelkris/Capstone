using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    //public GameObject winnerUI;
    public GameObject player1SpawnPosition;
    public GameObject player2SpawnPosition;

    private void Awake()
    {
        if (GameStateController.isPlayerOne)
        {
            PhotonNetwork.Instantiate("PlayerOne", player1SpawnPosition.transform.position,
                        Quaternion.identity);

            // Hide the player 2 canvas object from player 1
            GameObject.Find("CanvasPlayer2").SetActive(false);
        }
        else
        {
            GameObject player2 = PhotonNetwork.Instantiate("PlayerThree", player2SpawnPosition.transform.position, Quaternion.identity);
            GameObject.Find("CanvasPlayer1").SetActive(false);
            GameObject.Find("MinimapCamera").GetComponent<MinimapFollower>().enabled = true;

        }
    }
}
