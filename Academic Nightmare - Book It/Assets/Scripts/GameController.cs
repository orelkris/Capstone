using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    //public GameObject winnerUI;
    public GameObject player1SpawnPosition;
    public GameObject player2SpawnPosition;
    public static List<LocationTracker> ListOflocationColour;
    public static List<GameObject> ListOfSymbols;
    public static int numOfSymbols = 4;
    public static int correctSymbolIndex = 0;
    //public GameObject symbolSpawnPosition;

    private void Awake()
    {
        ListOfSymbols = new List<GameObject>();
        ListOflocationColour = new List<LocationTracker>();

        if (GameStateController.isPlayerOne)
        {
            /*PhotonNetwork.Instantiate("Player", player1SpawnPosition.transform.position,
                        Quaternion.identity);

            // Hide the player 2 canvas object from player 1
            GameObject.Find("PanelCode").SetActive(false);
            GameObject.Find("Crosshair").SetActive(false);*/

            Transform[] spawnLocation = GameObject.Find("Symbol Location Holder").GetComponentsInChildren<Transform>();
            
            //keep track of symbol location and the shelf colour they are associated with
            for (int i = 1; i < spawnLocation.Length; i++)
            {
                //Debug.Log("SHELF COLOUR : " + ((Material)Resources.Load($"Materials/Shelf/{spawnLocation[i].tag}", typeof(Material))).name);
                ListOflocationColour.Add(new LocationTracker(spawnLocation[i].transform.position,
                    ((Material)Resources.Load($"Materials/Shelf/{spawnLocation[i].tag}", typeof(Material)))));
            }

            Shuffle(ListOflocationColour, 1, ListOflocationColour.Count);

            //Debug.Log(spawnLocation[1].transform.position);
            //Debug.Log(spawnLocation[2].transform.position);
            //Attempting to instantiate a symbol at a specified location
            //getting a list of code
            for(int i = 0; i < numOfSymbols; i++)
            {
                PhotonNetwork.InstantiateRoomObject($"Symbol{i + 1}", ListOflocationColour[i].location, Quaternion.identity);
                ListOfSymbols.Add(GameObject.Find($"Symbol{i + 1}(Clone)"));
            }

            Shuffle(ListOfSymbols, 0, ListOfSymbols.Count);
            
        }
 /*       else
        {
            PhotonNetwork.Instantiate("Player", player2SpawnPosition.transform.position, Quaternion.identity);
            //GameObject.Find("MinimapCamera").GetComponent<MinimapFollower>().enabled = true;

        }*/
    }

    public static Material FindColour(Vector3 v)
    {
        foreach(var lc in ListOflocationColour)
        {
            if(lc.equals(v))
            {
                return lc.colour;
            }
        }

        return null;
    }

    //make sure to seed the random generator with a fixed seed for testing purposes
    //otherwise you can see it with time and thus ensuring it is always different
    public List<T> Shuffle<T>(List<T> list, int startIndex, int endIndex)
    {
        System.Random rnd = new System.Random();
        for (int i = startIndex ; i < endIndex; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }
}

public class LocationTracker
{
    public Vector3 location;
    public Material colour;

    public LocationTracker(Vector3 newLocation, Material newColour)
    {
        location = newLocation;
        colour = newColour;
    }

    public bool equals(Vector3 v2)
    {
        return (location.x == v2.x &&
            location.y == v2.y &&
            location.z == v2.z);
    }
    
}
