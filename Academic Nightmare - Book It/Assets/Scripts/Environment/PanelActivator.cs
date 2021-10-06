using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public GameObject panelToActivate;

    public GameObject panelToDeactivate;

    public GameObject panelBlocking;

    //this function will be called when the button is clicked to switch a panel
    // hook up this function to onClick of the button via inspector panel
    public void AnimatePanelHide()
    {
        if (this.name == "ButtonHomepage")
        {
            CellphoneView.currentPanelIndex = 0;
            Debug.Log("ButtonHomepage Pressed " + CellphoneView.currentPanelIndex);
        }
        else if (this.name == "ButtonMinimap")
        {
            CellphoneView.currentPanelIndex = 1;
            Debug.Log("ButtonMinimap Pressed " + CellphoneView.currentPanelIndex);
        }
        else if (this.name == "ButtonSymbols")
        {
            CellphoneView.currentPanelIndex = 2;
            Debug.Log("ButtonSymbol Pressed " + CellphoneView.currentPanelIndex);
        }
        else if (this.name == "ButtonCompass")
        {
            CellphoneView.currentPanelIndex = 3;
            Debug.Log("ButtonCompass Pressed " + CellphoneView.currentPanelIndex);
        }

        StartCoroutine(Deactivate(1.0f));
        // StartCoroutine("Deactivate", 1.0f);
        // why do this? Not a good idea       
    }

    // what is a coroutine?
    // coroutine allows you to stop execution 
    // wait for a bit and continue execution ?
    private IEnumerator Deactivate(float delay)
    {

        //panelToDeactivate.GetComponent<Animator>().SetTrigger("changeState");

        //important line!!!
        // 1 means 1 second
        // notice this is associated with IEnumerator
        yield return new WaitForSeconds(delay);//means put this funciton on sleep before continuing

        // play slideout animation
        // wait for 1 second
        // turn off current panel
        // turn on the other panel
        if (!HotSpot.downloadComplete && (CellphoneView.currentPanelIndex == 1 || CellphoneView.currentPanelIndex == 2))
        {
            CellphoneView.currentPanelIndex = 4;

            panelToDeactivate.SetActive(false);

            panelBlocking.SetActive(true);
        }
        else
        {

            panelToDeactivate.SetActive(false);

            panelToActivate.SetActive(true);
        }


    }
}


