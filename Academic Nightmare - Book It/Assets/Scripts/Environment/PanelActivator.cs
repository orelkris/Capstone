using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public GameObject panelToActivate;

    public GameObject panelToDeactivate;

    //this function will be called when the button is clicked to switch a panel
    // hook up this function to onClick of the button via inspector panel
    public void AnimatePanelHide()
    {
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
        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);
    }


    /// ALTERNATE WAY

    //Animate Panel Hide Alternate
    public void AnimatePanelHideAlternate()
    {
        //panelToDeactivate.GetComponent<Animator>().SetTrigger("changeState");
        Invoke("HideAndActivatePanelAfterDelay", 1.0f);//calls after 1 second of delay
    }


    void HideAndActivatePanelAfterDelay()
    {
        // play slideout animation
        // wait for 1 second
        // turn off current panel
        // turn on the other panel
        panelToDeactivate.SetActive(false);

        panelToActivate.SetActive(true);
    }
}


