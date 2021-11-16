using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivatorThief : MonoBehaviour
{
    public GameObject alwaysActivateCellphone;

    public GameObject panelToActivate;

    public GameObject panelToDeactivate;

    public GameObject CanvasThief;

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

        panelToDeactivate = CanvasThief.GetComponent<CellphoneManagerThief>().cellphonePanels[CellphoneManagerThief.currentPanelIndex];

        alwaysActivateCellphone.SetActive(true);

        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);

        CellphoneManagerThief.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;

    }

    // what is a coroutine?
    // coroutine allows you to stop execution 
    // wait for a bit and continue execution ?
    private IEnumerator Deactivate(float delay)
    {
        CellphoneManagerThief.currentPanelIndex = panelToActivate.GetComponent<SelfPanelIndex>().SelfIndex;

        //important line!!!
        // 1 means 1 second
        // notice this is associated with IEnumerator
        yield return new WaitForSeconds(delay);//means put this funciton on sleep before continuing

        alwaysActivateCellphone.SetActive(true);

        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);
        
    }
}
