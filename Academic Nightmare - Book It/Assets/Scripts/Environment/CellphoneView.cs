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
                        cellphonePanels[currentPanelIndex].SetActive(true);

                        /*
                        if (HotSpot.downloadComplete)
                        {
                            cellphonePanels[2].SetActive(true);
                            cellphonePanels[currentPanelIndex].SetActive(false);
                        }
                        else 
                        {
                            if(currentPanelIndex == 1 || currentPanelIndex == 2)
                            {
                                cellphonePanels[4].SetActive(true);
                            }
                            else
                            {
                                cellphonePanels[currentPanelIndex].SetActive(true);
                            }
                        }
                        */




                        cellphoneVisible = true;
                        //cellphonePanel.SetActive(true);
                    }
                    else
                    {
                        cellphonePanels[currentPanelIndex].SetActive(false);
                        Debug.Log("CURRENT INDEX " + currentPanelIndex);
                        /*
                        if(HotSpot.downloadComplete)
                        {
                            cellphonePanels[currentPanelIndex].SetActive(false);
                        }
                        else
                        {
                            if (currentPanelIndex == 1 || currentPanelIndex == 2)
                            {
                                cellphonePanels[4].SetActive(false);
                            }
                            else
                            {
                                cellphonePanels[currentPanelIndex].SetActive(false);

                            }
                            //cellphonePanels[4].SetActive(false);
                        }
                        */                       


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
