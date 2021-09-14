using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        if (GameStateController.isPlayerOne)
        {
            PhotonNetwork.Instantiate("PlayerOne", new Vector3(1f, 1.5f, 1f), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerTwo", new Vector3(-1f, 1.5f, -1f), Quaternion.identity);
        }
    }
}
