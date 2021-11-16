using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivatorHacker : MonoBehaviour
{
    public GameObject alwaysActivateCellphone;

    public GameObject panelToActivate;

    public GameObject panelToDeactivate;

    public GameObject panelBlocking;

    public GameObject symbolsPanel;

    public GameObject CanvasHacker;

    //this function will be called when the button is clicked to switch a panel
    // hook up this function to onClick of the button via inspector panel
    public void AnimatePanelHide()
    {

        StartCoroutine(Deactivate(1.0f));    
    }

    public void Homepage()
    {

        StartCoroutine(HomepageActivator(1.0f));
    }

    private IEnumerator HomepageActivator(float delay)
    {
        yield return new WaitForSeconds(delay);

        panelToDeactivate = CanvasHacker.GetComponent<CellphoneManagerHacker>().cellphonePanels[CellphoneManagerHacker.currentPanelIndex];

        alwaysActivateCellphone.SetActive(true);

        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);

        CellphoneManagerHacker.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;
        
    }

    // what is a coroutine?
    // coroutine allows you to stop execution 
    // wait for a bit and continue execution ?
    private IEnumerator Deactivate(float delay)
    {
        CellphoneManagerHacker.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;

        //important line!!!
        // 1 means 1 second
        // notice this is associated with IEnumerator
        yield return new WaitForSeconds(delay);//means put this funciton on sleep before continuing

        alwaysActivateCellphone.SetActive(true);

        if (!HotSpot.downloadComplete && (CellphoneManagerHacker.currentPanelIndex == symbolsPanel.GetComponent<SelfPanelIndex>().SelfIndex))
        {
            panelToDeactivate.SetActive(false);

            panelBlocking.SetActive(true);

            CellphoneManagerHacker.currentPanelIndex = panelBlocking.GetComponent<SelfPanelIndex>().SelfIndex;
        }
        else
        {

            panelToDeactivate.SetActive(false);

            panelToActivate.SetActive(true);
        }
    }
}


