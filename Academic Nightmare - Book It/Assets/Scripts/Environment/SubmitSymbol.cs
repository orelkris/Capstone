using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SubmitSymbol : MonoBehaviour
{
    public GameObject gameManager;
    GameObject phone;
    GameObject downloadPanel;
    GameController gm;

    private void Start()
    {
        gm = new GameController();
        gameManager = GameObject.Find("GameController");
        phone = GameObject.Find("CanvasPlayerOne(Clone)");
        downloadPanel = phone.GetComponent<CellphoneView>().downloadPanel;


    }
    public void Submit()
    {
        string name = GameController.ListOfSymbols[GameController.correctSymbolIndex].GetComponent<SymbolInformation>().selfObject.m_name;
        if (this.GetComponent<Image>().sprite.name == name)
        {
            Debug.Log("Correct");
            int temp = GameController.symbolsFound + 1;

            //GameController.symbolsFound++;
            PhotonView pv = gameManager.GetComponent<PhotonView>();
            pv.RPC("SymbolsFound", RpcTarget.All, temp);
            Debug.Log("Found Symbols " + GameController.symbolsFound);

            if (GameController.symbolsFound == 2)
            {
                GameObject.Find("PanelReverseWarning").GetComponent<Animator>().SetBool("isHidden", false);
            }
            //advance the correct symbol
            GameController.correctSymbolIndex++;
            GameObject.Find("SymbolHolderPanel").GetComponent<LoadSymbolImage>().LoadColourCode();

            // reset the downloading bar
            HotSpot.downloadComplete = false;
            phone.GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex].SetActive(false);
            CellphoneView.currentPanelIndex = downloadPanel.GetComponent<SelfPanelIndex>().SelfIndex;
            phone.GetComponent<CellphoneView>().cellphonePanels[CellphoneView.currentPanelIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect");
            //Debug.Log(this.GetComponent<Image>().sprite.name);
        }
    }
}
