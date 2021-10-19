using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadSymbolImage : MonoBehaviour
{

    GameObject symbolButton;
    GameObject panelSymbolHolder;
    public Button button;
    Image imageSymbolColour;
    TextMeshProUGUI textSymbolCode;
    bool first = true;


    private void Start()
    {

        // find the symbol holding panel
        panelSymbolHolder = GameObject.Find("SymbolHolderPanel");
        imageSymbolColour = GameObject.Find("ShelfColourImage").GetComponent<Image>();

        // this is the object to use when accessing the TextMeshPro text item
        textSymbolCode = GameObject.Find("CodeText").GetComponent<TextMeshProUGUI>();

        LoadSymbols();
    }

    public void LoadSymbols()
    {
        LoadColourCode();
        // Hacker screen symbol list
        for (int i = 0; i <= GameController.numOfSymbols; i++)
        {

            // instantiate a button prefab and assign the symbol sprites to it
            Instantiate<Button>(button, panelSymbolHolder.transform);
            button.GetComponent<Image>().sprite = (Sprite)Resources.Load($"Materials/Symbols/Symbol{i + 1}", typeof(Sprite));

        }

        // ask about why this extra button is appearing...super weird stuff!!!!!!!!!!
        DestroyImmediate(panelSymbolHolder.GetComponentsInChildren<Button>()[0].gameObject);
    }

    public void LoadColourCode()
    {
        //Debug.Log("LOAD SYMBOL " + GameController.ListOfSymbols[GameController.correctSymbolIndex].name);
        imageSymbolColour.color = GameController.ListOfSymbols[GameController.correctSymbolIndex].GetComponent<SymbolInformation>().shelfColour.color;

        textSymbolCode.SetText(GameController.ListOfSymbols[GameController.correctSymbolIndex].GetComponent<SymbolInformation>().selfCode);
    }
}
