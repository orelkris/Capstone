using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviour
{
    public bool isPlayerOne;

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
            Destroy(GameObject.FindGameObjectWithTag("Main Camera"));
        }
    }
}
