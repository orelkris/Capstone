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
    GameObject hotSpot;

    private void Start()
    {
        gm = new GameController();
        gameManager = GameObject.Find("GameController");
        phone = GameObject.FindGameObjectWithTag("CellphoneHacker");
        downloadPanel = phone.GetComponent<CellphoneManagerHacker>().downloadPanel;


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

            if (GameController.symbolsFound == 1)
            {
                GameObject.Find("PanelReverseWarning").GetComponent<Animator>().SetBool("isHidden", false);
            }
            //advance the correct symbol
            GameController.correctSymbolIndex++;
            GameObject.Find("SymbolHolderPanel").GetComponent<LoadSymbolImage>().LoadColourCode();

            // reset the downloading bar
            HotSpot.downloadComplete = false;
            hotSpot = GameObject.FindGameObjectWithTag("HotSpot");
            hotSpot.GetComponent<HotSpot>().SpawnHotSpot();
            phone.GetComponent<CellphoneManagerHacker>().cellphonePanels[CellphoneManagerHacker.currentPanelIndex].SetActive(false);
            CellphoneManagerHacker.currentPanelIndex = downloadPanel.GetComponent<SelfPanelIndex>().SelfIndex;
            phone.GetComponent<CellphoneManagerHacker>().cellphonePanels[CellphoneManagerHacker.currentPanelIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Incorrect");
        }
    }
}
