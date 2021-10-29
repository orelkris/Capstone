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

    // 
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

    void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["class"].Equals("hacker"))
        {
            PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "Player"), 
                spawnPositions[0], 
                Quaternion.identity
            );

            //GameObject.FindGameObjectWithTag("Player").gameObject.tag = "Hacker";
            //Debug.Log(GameObject.FindGameObjectWithTag("Player").tag);
            PhotonNetwork.Instantiate(
                Path.Combine("AI", "Librarian"),
                AISpawnPositions[0],
                Quaternion.identity
            );

            // Hide the player 2 canvas object from player 1
            GameObject.Find("PanelCode").SetActive(false);
            GameObject.Find("Crosshair").SetActive(false);
        }
        else
        {
            GameObject player =
            PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "Player"), 
                spawnPositions[1], 
                Quaternion.identity
            );

            PhotonNetwork.Instantiate(
                Path.Combine("AI", "Assistant"),
                AISpawnPositions[1],
                Quaternion.identity
            );

            //default tag is Hacker
            //GameObject.FindGameObjectWithTag("Hacker").tag = "Thief";
            player.transform.GetChild(2).tag = "Thief";
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
