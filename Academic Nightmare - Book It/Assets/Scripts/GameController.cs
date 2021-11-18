    using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //public GameObject winnerUI;
    public static List<LocationTracker> ListOflocationColour;
    public static List<GameObject> ListOfSymbols;
    public static int numOfSymbols = 4;
    public static int correctSymbolIndex = 0;

    public Animator finishedPanel;
    public ParticleSystem successParticlesHacker;
    public ParticleSystem successParticlesThief;

    public static int symbolsFound;

    private bool onlyOneCanvas = false;


    //Button btn;
    //public GameObject symbolSpawnPosition;

    private void Start()
    {
        symbolsFound = 0;

        ListOfSymbols = new List<GameObject>();
        ListOflocationColour = new List<LocationTracker>();

        //PhotonNetwork.Instantiate("CanvasHacker", Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate("CanvasThief", Vector3.zero, Quaternion.identity);

        //PhotonNetwork.Instantiate("HotSpot", Vector3.zero, Quaternion.identity);

 /*       if (GameStateController.isPlayerOne)
        {*/
        Transform[] spawnLocation = GameObject.Find("Symbol Location Holder").GetComponentsInChildren<Transform>();
            
        //keep track of symbol location and the shelf colour they are associated with
        for (int i = 1; i < spawnLocation.Length; i++)
        {
            //Debug.Log("SHELF COLOUR : " + ((Material)Resources.Load($"Materials/Shelf/{spawnLocation[i].tag}", typeof(Material))).name);
            ListOflocationColour.Add(new LocationTracker(spawnLocation[i].transform.position,
                ((Material)Resources.Load($"Materials/Shelf/{spawnLocation[i].tag}", typeof(Material)))));
        }

        //Shuffle(ListOflocationColour, 1, ListOflocationColour.Count);

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

        //btn = GameObject.Find("MainMenuButton").GetComponent<Button>();
        //btn.onClick.AddListener(LoadMainMenu);

        /*}*/
    }

    private void Update()
    {
        
        if(GameObject.Find("FinishedPanel") != null)
        {
            finishedPanel = GameObject.Find("FinishedPanel").GetComponent<Animator>();
            if(GameObject.Find("Success Particles") != null)
            {
                successParticlesHacker = GameObject.Find("Success Particles").GetComponent<ParticleSystem>();
            }
            if(GameObject.Find("Success Particles Thief") != null)
            {
                successParticlesThief = GameObject.Find("Success Particles Thief").GetComponent<ParticleSystem>();
            }
        }        

        

        if(GameController.symbolsFound == 3)
        {
            if(GameObject.Find("MainPanel") == null)
            {

            }
            else
            {
                GameObject.Find("MainPanel").SetActive(false);
            }

            finishedPanel.SetTrigger("finished");
            successParticlesHacker.Play();
            successParticlesThief.Play();
        }
    }

    //no idea how to reload the main menu level using photon!
    //public void LoadMainMenu()
    //{

       // if(PhotonNetwork.IsMasterClient)
       // {
        //    PhotonNetwork.LoadLevel(0);
        //}
        // get the current build of a level and add 1 to it. Can be reused for further level
        // transitions for ease
        //StartCoroutine(LoadLevelAsync(0));
   // }

    // we do not want to load the level right away, we want
    // to have enough time to play the load animation
    System.Collections.IEnumerator LoadLevelAsync(int levelIndex)
    {
        //Play animation
        finishedPanel.SetTrigger("retreat");
        //Wait
        yield return new WaitForSeconds(2);

        //Load scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
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

    [PunRPC]
    public void SymbolsFound(int temp)
    {
        GameController.symbolsFound = temp;
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
