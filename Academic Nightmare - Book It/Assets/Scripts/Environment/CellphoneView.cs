using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellphoneView : MonoBehaviour
{
    public static bool cellphoneVisible = false;
    public GameObject cellphonePanel;
    public List<GameObject> cellphonePanels;
    public static int currentPanelIndex = 0;
    public GameObject slider;
    public GameObject cover;

    public void Update()
    {
        if(GameStateController.isPlayerOne)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {


                Animator animator = this.GetComponent<Animator>();

                if (animator != null)
                {
                    //bool isOpen = animator.GetBool("open");
                    if (!cellphoneVisible)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.Confined;
                        PlayerController.canPlayerOneMove = false;

                        if (HotSpot.downloadComplete)
                        {
                            cellphonePanels[currentPanelIndex].SetActive(true);
                            cellphonePanels[3].SetActive(false);
                        }
                        else
                        {
                            cellphonePanels[3].SetActive(true);
                        }


                        cellphoneVisible = true;
                        //cellphonePanel.SetActive(true);
                    }
                    else
                    {
                        if(HotSpot.downloadComplete)
                        {
                            cellphonePanels[currentPanelIndex].SetActive(false);
                        }
                        else
                        {
                            cellphonePanels[3].SetActive(false);
                        }
                        cellphoneVisible = false;

                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        PlayerController.canPlayerOneMove = true;
                    }

                    //animator.SetBool("open", !isOpen);
                }
            }
        }
       

    }
}
