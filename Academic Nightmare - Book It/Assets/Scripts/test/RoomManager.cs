using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private Player[] players;

    //temp spawn positions
    private Vector3[] spawnPositions = {
        new Vector3(-1, 54, 41),
        new Vector3(-1, 3, 79),
    };

    private Vector3[] AISpawnPositions =
    {
        new Vector3(-1, 54, 41),
        new Vector3(2, -1, 40)
    };

    readonly string hackerPrefabPath = Path.Combine("PhotonPrefabs", "Hacker");
    readonly string theifPrefabPath = Path.Combine("PhotonPrefabs", "Thief");
    readonly string ghostPrefabPath = Path.Combine("PhotonPrefabs", "GhostCam");

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

    }

    public override void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    public override void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) => SpawnPlayer();

    public void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["class"].Equals("ghost"))
        {
            PhotonNetwork.Instantiate(
                ghostPrefabPath,
                spawnPositions[0],
                Quaternion.identity
            );
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties["class"].Equals("hacker"))
        {
            PhotonNetwork.Instantiate(
                hackerPrefabPath, 
                spawnPositions[0], 
                Quaternion.identity
            );

            PhotonNetwork.Instantiate(
                Path.Combine("AI", "Librarian"),
                AISpawnPositions[0],
                Quaternion.identity
            );

            // Hide the player 2 canvas object from player 1
            if (GameObject.Find("PanelCode") && GameObject.Find("Crosshair"))
            {
                GameObject.Find("PanelCode").SetActive(false);
                GameObject.Find("Crosshair").SetActive(false);
            }

            PhotonNetwork.Instantiate("HotSpot", Vector3.zero, Quaternion.identity);
        }
        else if(PhotonNetwork.LocalPlayer.CustomProperties["class"].Equals("thief"))
        {
            PhotonNetwork.Instantiate(
                theifPrefabPath, 
                spawnPositions[1], 
                Quaternion.identity
            );

            PhotonNetwork.Instantiate(
                Path.Combine("AI", "Assistant"),
                AISpawnPositions[1],
                Quaternion.identity
            );
        }
    }

    /*
    -To Do-
    
    Initialize
        - Spawn Enemies
        - Spawn Books (symbols)

    Game Flow
        - Game State management (pause, cut scene, so on..)
        - Event listening and triggers
        - 

    */
}
