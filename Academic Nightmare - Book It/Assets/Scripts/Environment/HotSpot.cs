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

    //PHONE Canvas//
    GameObject phone;

    void Start()
    {
        downloadComplete = false;

        Vector3 spot1 = new Vector3 { x = 56f, y = 55f, z = -48f };

        Vector3 spot2 = new Vector3 { x = 56f, y = 55f, z = -133f };

        Vector3 spot3 = new Vector3 { x = -60f, y = 55f, z = -133f };

        Vector3 spot4 = new Vector3 { x = -60f, y = 55f, z = 94f };

        HotSpotLocation.Add(spot1);
        HotSpotLocation.Add(spot2);
        HotSpotLocation.Add(spot3);
        HotSpotLocation.Add(spot4);

        hotSpotIndex = 0;

        hotSpot = this.gameObject;

        counter = 0f;
        canDownload = 0f;

        player = GameObject.FindGameObjectWithTag("Hacker");

        SpawnHotSpot();

        //phone = GameObject.Find("CanvasHacker");
        phone = GameObject.FindGameObjectWithTag("CellphoneHacker");

        symbolPanel = phone.GetComponent<CellphoneManagerHacker>().symbolPanel;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hacker")
        {
            hasWifi = true;

            if (GameObject.Find("DownloadPanel") != null)
            {
                if (CellphoneManagerHacker.currentPanelIndex == phone.GetComponent<CellphoneManagerHacker>().downloadPanel.GetComponent<SelfPanelIndex>().SelfIndex
                        && !downloadComplete && player.tag == "Hacker")
                {
                    // download image only if load image is active
                    // this happens only when the image has not yet been downloaded

                    //********LOADBAR*******/

                    // just in case the player turned off the cellphone, always check for null values
                    if (GameObject.Find("ProgressSlider") != null)
                    {
                        canDownload += Time.deltaTime;

                        progressBar = GameObject.Find("ProgressSlider").GetComponent<Slider>();

                        progressBar.value = 0.1f * Mathf.Round(canDownload);
                        Debug.Log("PROGRESS " + progressBar.value);

                        if (progressBar.value == 1)
                        {
                            downloadComplete = true;
                            canDownload = 0;
                            progressBar.value = 0;

                            phone.GetComponent<CellphoneManagerHacker>().cellphonePanels[CellphoneManagerHacker.currentPanelIndex].SetActive(false);
                            CellphoneManagerHacker.currentPanelIndex = symbolPanel.GetComponent<SelfPanelIndex>().SelfIndex;
                            phone.GetComponent<CellphoneManagerHacker>().cellphonePanels[CellphoneManagerHacker.currentPanelIndex].SetActive(true);

                        }
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        hasWifi = false;
    }

    [PunRPC]
    public void SpawnHotSpot()
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
}