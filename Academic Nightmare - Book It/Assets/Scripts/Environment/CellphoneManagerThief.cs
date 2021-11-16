using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CellphoneManagerThief : MonoBehaviour
{
    public static bool cellphoneVisible = false;

    //player 1 cellphone
    public GameObject cellphoneApps;

    public List<GameObject> cellphonePanels;

    public int cellphoneAlwaysActive = 0;
    //public int alwaysActive = 1;
    public static int currentPanelIndex = 2;

    public GameObject homepagePanel;
    public GameObject compassPanel;

    Button btn;
    private void Start()
    {
        cellphoneVisible = false;
        currentPanelIndex = 2;

        btn = GameObject.Find("MainMenuButton").GetComponent<Button>();
        btn.onClick.AddListener(LoadMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (!cellphoneVisible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                cellphonePanels[cellphoneAlwaysActive].SetActive(true);

                cellphonePanels[currentPanelIndex].SetActive(true);
                //Debug.Log("INDEX " + currentPanelIndex);
                cellphoneVisible = true;
            }
            else
            {
                cellphonePanels[cellphoneAlwaysActive].SetActive(false);

                cellphonePanels[currentPanelIndex].SetActive(false);

                cellphoneVisible = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
    }

    public void LoadMainMenu()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(0);
        }
        // get the current build of a level and add 1 to it. Can be reused for further level
        // transitions for ease
        //StartCoroutine(LoadLevelAsync(0));
    }
}
