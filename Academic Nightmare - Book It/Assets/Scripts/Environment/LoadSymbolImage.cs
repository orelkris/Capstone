using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSymbolImage : MonoBehaviour
{

    GameObject symbolButton;
    GameObject panelSymbolHolder;
    Button button;

    private void Start()
    {
        // find the symbol holding panel
        panelSymbolHolder = GameObject.Find("PanelSymbolHolder");
        
        LoadSymbols();
    }
    public void LoadSymbols()
    {
        
        // Hacker screen symbol list
        for (int i = 0; i < GameEnvironment.Singleton.GetNumOfSymbols; i++)
        {
            // add buttons dynamically to the panel
            // this way we can add as many buttons as needed in the future if
            // we choose to add more symbols
            button = panelSymbolHolder.AddComponent<Button>();
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            button.GetComponentInChildren<Text>().text = "";
            // find the symbol sprites and apply them to the newly created buttons
            //button = GameObject.Find($"ButtonSymbol{i}");
            button.GetComponent<Image>().sprite = (Sprite)Resources.Load($"Materials/Images/Symbol{i + 1}", typeof(Sprite));
        }
    }
}
