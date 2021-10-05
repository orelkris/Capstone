using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitSymbol : MonoBehaviour
{
    public void Submit()
    {
        string name = GameController.ListOfSymbols[GameController.correctSymbolIndex].GetComponent<SymbolInformation>().selfObject.m_name;
        if (this.GetComponent<Image>().sprite.name == name)
        {
            Debug.Log("Correct");
            //advance the correct symbol
            GameController.correctSymbolIndex++;
            GameObject.Find("PanelSymbolHolder").GetComponent<LoadSymbolImage>().LoadColourCode();
        }
        else
        {
            Debug.Log("Incorrect");
            //Debug.Log(this.GetComponent<Image>().sprite.name);
        }
    }
}
