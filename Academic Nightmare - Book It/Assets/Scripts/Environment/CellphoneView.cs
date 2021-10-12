using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneView : MonoBehaviour
{
    public static bool cellphoneVisible = false;
    public static bool cellPhoneVisiblePlayerTwo = false;

    //player 1 cellphone
    public GameObject playerOneCellphone;

    //player 2 cellphone
    public GameObject playerTwoCellphone;

    public GameObject cellphonePanel;
    public List<GameObject> cellphonePanels;
    public List<GameObject> cellphonePanelsPlayerTwo;

    public int alwaysActive = 0;
    public static int currentPanelIndex = 1;
    public static int currentPanelIndexPlayerTwo = 1;

    public GameObject slider;
    public GameObject symbolPanel;
    public GameObject downloadPanel;

    public void Update()
    {
        if(GameStateController.isPlayerOne)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //bool isOpen = animator.GetBool("open");
                if (!cellphoneVisible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    //PlayerController.canPlayerOneMove = false;
                    cellphonePanels[alwaysActive].SetActive(true);
                    //playerTwoCellphone.SetActive(false);
                    cellphonePanels[currentPanelIndex].SetActive(true);
                    Debug.Log("INDEX " + currentPanelIndex);
                    cellphoneVisible = true;
                }
                else
                {
                    cellphonePanels[alwaysActive].SetActive(false);
                    cellphonePanels[currentPanelIndex].SetActive(false);
                    Debug.Log("CURRENT INDEX " + currentPanelIndex);                    


                    cellphoneVisible = false;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    //PlayerController.canPlayerOneMove = true;
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                
                //bool isOpen = animator.GetBool("open");
                if (!cellPhoneVisiblePlayerTwo)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    //PlayerController.canPlayerOneMove = false;
                    cellphonePanelsPlayerTwo[alwaysActive].SetActive(true);
                    Debug.Log("ALWAYS ACTIVE " + currentPanelIndexPlayerTwo);
                    playerOneCellphone.SetActive(false);
                    cellphonePanelsPlayerTwo[currentPanelIndexPlayerTwo].SetActive(true);
                    Debug.Log("Player Two INDEX " + currentPanelIndexPlayerTwo);
                    cellPhoneVisiblePlayerTwo = true;
                }
                else
                {
                    cellphonePanelsPlayerTwo[alwaysActive].SetActive(false);
                    cellphonePanelsPlayerTwo[currentPanelIndexPlayerTwo].SetActive(false);
                    Debug.Log("Player Two CURRENT INDEX " + currentPanelIndexPlayerTwo);

                    cellPhoneVisiblePlayerTwo = false;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    //PlayerController.canPlayerOneMove = true;
                }
            }
            
        }
    }
}

