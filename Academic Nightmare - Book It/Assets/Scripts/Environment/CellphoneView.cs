using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneView : MonoBehaviour
{
    public static bool cellphoneVisible = false;
    public GameObject cellphonePanel;
    public List<GameObject> cellphonePanels;
    public int alwaysActive = 0;
    public static int currentPanelIndex = 1;
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
                    PlayerController.canPlayerOneMove = false;
                    cellphonePanels[alwaysActive].SetActive(true);
                    cellphonePanels[currentPanelIndex].SetActive(true);

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
                    PlayerController.canPlayerOneMove = true;
                }
        }
        }
    }
}

