using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public GameObject alwaysActivate;

    public GameObject panelToActivate;

    public GameObject panelToDeactivate;

    public GameObject panelBlocking;

    public GameObject symbolsPanel;

    public GameObject phone;

    //this function will be called when the button is clicked to switch a panel
    // hook up this function to onClick of the button via inspector panel
    public void AnimatePanelHide()
    {

        StartCoroutine(Deactivate(1.0f));
        // StartCoroutine("Deactivate", 1.0f);
        // why do this? Not a good idea       
    }

    public void Homepage()
    {

        StartCoroutine(HomepageActivator(1.0f));
    }

    private IEnumerator HomepageActivator(float delay)
    {
        yield return new WaitForSeconds(delay);

        panelToDeactivate = phone.GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex];

        alwaysActivate.SetActive(true);

        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);

        CellphoneView.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;
    }

    // what is a coroutine?
    // coroutine allows you to stop execution 
    // wait for a bit and continue execution ?
    private IEnumerator Deactivate(float delay)
    {
        CellphoneView.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;

        //important line!!!
        // 1 means 1 second
        // notice this is associated with IEnumerator
        yield return new WaitForSeconds(delay);//means put this funciton on sleep before continuing

        // play slideout animation
        // wait for 1 second
        // turn off current panel
        // turn on the other panel

        alwaysActivate.SetActive(true);

        if (!HotSpot.downloadComplete && (CellphoneView.currentPanelIndex == symbolsPanel.GetComponent<SelfPanelIndex>().SelfIndex))
        {
            //CellphoneView.currentPanelIndex = 3;

            panelToDeactivate.SetActive(false);

            panelBlocking.SetActive(true);

            CellphoneView.currentPanelIndex = panelBlocking.GetComponent<SelfPanelIndex>().SelfIndex;
        }
        else
        {

            panelToDeactivate.SetActive(false);

            panelToActivate.SetActive(true);
        }
    }
}


