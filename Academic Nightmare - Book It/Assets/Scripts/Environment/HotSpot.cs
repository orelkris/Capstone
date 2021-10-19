using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    public Slider progressBar;
    public GameObject symbolPanel;

    public static Animator animator;


    //***********LOADBAR********//
    public static GameObject loadImagePanel;
    //public static Slider progressBar;

    // random list of colours
    Color32[] colorList = { Color.red, Color.blue, Color.green, Color.magenta };
    // Start is called before the first frame update

    //PHONE Canvas//
    GameObject phone;

    void Start()
    {
        Vector3 spot1 = new Vector3 { x = 56f, y = 55f, z = -48f };

        Vector3 spot2 = new Vector3 { x = 56f, y = 55f, z = -133f };

        Vector3 spot3 = new Vector3 { x = -60f, y = 55f, z = -133f };

        Vector3 spot4 = new Vector3 { x = -60f, y = 55f, z = 94f };

        HotSpotLocation.Add(spot1);
        HotSpotLocation.Add(spot2);
        HotSpotLocation.Add(spot3);
        HotSpotLocation.Add(spot4);

        hotSpotIndex = 0;

        //audioHotSpot.GetComponent<AudioSource>();
        counter = 0f;
        canDownload = 0f;

        player = GameObject.FindGameObjectWithTag("Hacker");

        // create a sphere object
        hotSpot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hotSpot.GetComponent<MeshRenderer>().enabled = false;

        SpawnHotSpot();

        if (GameStateController.isPlayerOne)
        {
            //***********LOADBAR********//
            //loadImagePanel = GameObject.Find("CanvasGlobal").GetComponent<CellphoneView>().cover;
            progressBar = GameObject.Find("CanvasPlayerOne(Clone)").GetComponent<CellphoneView>().slider.GetComponent<Slider>();

            phone = GameObject.Find("CanvasPlayerOne(Clone)");

            symbolPanel = GameObject.Find("CanvasPlayerOne(Clone)").GetComponent<CellphoneView>().symbolPanel;
        }



    }

    [PunRPC]
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
        if (player != null)
        {
            counter += Time.deltaTime;

            if (this.GetComponent<Collider>().bounds.Contains(player.transform.position))
            {

                hasWifi = true;
                Debug.Log("Has Wifi");

                if (CellphoneView.currentPanelIndex == 4 && !downloadComplete && progressBar != null && GameStateController.isPlayerOne)
                {
                    // download image only if load image is active
                    // this happens only when the image has not yet been downloaded

                    canDownload += Time.deltaTime;


                    //********LOADBAR*******//
                    progressBar = GameObject.Find("ProgressSlider").GetComponent<Slider>();
                    progressBar.value = 0.1f * Mathf.Round(canDownload);
                    Debug.Log(progressBar.value);

                    if (progressBar.value == 1)
                    {
                        downloadComplete = true;
                        canDownload = 0;
                        progressBar.value = 0;

                        phone.GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex].SetActive(false);
                        CellphoneView.currentPanelIndex = symbolPanel.GetComponent<SelfPanelIndex>().SelfIndex;
                        phone.GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex].SetActive(true);

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

            if (Mathf.Round(counter) == 120)
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
}