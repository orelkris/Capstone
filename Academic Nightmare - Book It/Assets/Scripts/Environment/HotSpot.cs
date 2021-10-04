using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotSpot : MonoBehaviour
{
    GameObject hotSpot;
    GameObject player;
    public AudioSource audioHotSpot;
    public static bool hasWifi;
    bool audioToggle = true;
    float counter;
    public static float canDownload;
    public static int hotSpotIndex;
    public List<Vector3> HotSpotLocation = new List<Vector3>();
    public static bool downloadComplete = false;

    public static Animator animator;


    //***********LOADBAR********//
    public static GameObject loadImagePanel;
    public static Slider progressBar;

    // random list of colours
    Color32[] colorList = { Color.red, Color.blue, Color.green, Color.magenta };
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spot1 = new Vector3 { x = 12f, y = 3.5f, z = 33f };

        Vector3 spot2 = new Vector3 { x = 26f, y = 3.5f, z = -33f };

        Vector3 spot3 = new Vector3 { x = 22f, y = 3.5f, z = 39f };

        Vector3 spot4 = new Vector3 { x = 31f, y = 3.5f, z = 39f };

        HotSpotLocation.Add(spot1);
        HotSpotLocation.Add(spot2);
        HotSpotLocation.Add(spot3);
        HotSpotLocation.Add(spot4);

        hotSpotIndex = 0;

        //audioHotSpot.GetComponent<AudioSource>();
        counter = 0f;
        canDownload = 0f;

        player = GameObject.Find("PlayerOne(Clone)");

        // create a sphere object
        hotSpot = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        SpawnHotSpot();

        //***********LOADBAR********//
        loadImagePanel = GameObject.Find("CanvasGlobal").GetComponent<CellphoneView>().cover;
        progressBar = GameObject.Find("CanvasGlobal").GetComponent<CellphoneView>().slider.GetComponent<Slider>();

    }

    void SpawnHotSpot()
    {
        hotSpot.transform.position = new Vector3(HotSpotLocation[hotSpotIndex].x,
            HotSpotLocation[hotSpotIndex].y,
            HotSpotLocation[hotSpotIndex].z);

        hotSpot.transform.localScale = new Vector3(2f, 2f, 2f);

        // place the empty object where the new sphere is so that the player can collide with the hotSpot object to win
        this.transform.position = hotSpot.transform.position;

        hotSpotIndex++;

        if (hotSpotIndex == 4)
        {
            hotSpotIndex = 0;
        }
    }

    void Update()
    {
        counter += Time.deltaTime;

        if (this.GetComponent<Collider>().bounds.Contains(player.transform.position))
        {

            hasWifi = true;
            Debug.Log("Has Wifi");

            if(CellphoneView.cellphoneVisible && !downloadComplete)
            {
                // download image only if load image is active
                // this happens only when the image has not yet been downloaded

                canDownload += Time.deltaTime;


                //********LOADBAR*******//
                progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
                progressBar.value = 0.1f * Mathf.Round(canDownload);
                Debug.Log(progressBar.value);

                if (progressBar.value == 1)
                {
                    downloadComplete = true;
                    canDownload = 0;
                    GameObject.Find("CanvasGlobal").GetComponent<CellphoneView>().cellphonePanels[3].SetActive(false);
                    GameObject.Find("CanvasGlobal").GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex].SetActive(true);
                }
            }
        }
        else if (!(this.GetComponent<Collider>().bounds.Contains(player.transform.position)))
        {
            hasWifi = false;
        }

        //if (hasWifi && audioToggle)
       // {
            //audioHotSpot.Play();
         //   audioToggle = false;
       // }

        if (!hasWifi)
        {
            // audioHotSpot.Stop();
            //audioToggle = true;
            //loadImagePanel.SetActive(true);
        }

        if (Mathf.Round(counter) == 20)
        {
            SpawnHotSpot();
            counter = 0;
        }

        // if the random float is above 40, change the colour of the sphere based on the list of colours
        if (Mathf.Round(counter) % 2 == 0)
        {
            hotSpot.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else
        {
            hotSpot.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}