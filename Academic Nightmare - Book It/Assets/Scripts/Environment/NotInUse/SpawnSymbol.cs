using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class SpawnSymbol : MonoBehaviour
{
    public GameObject book;
    public GameObject player;
    int spawnTreasure = -1;
    int last = 0;
    GameObject temp;
    Text code;
    Material shelfColour;

    [PunRPC]
    public static List<SymbolObject> testSymbol = new List<SymbolObject>();

    void Start()
    {

        
        LoadSymbols();


        if (!GameStateController.isPlayerOne)
        {
            code = GameObject.Find("Code").GetComponent<Text>();
        }
    }

    [PunRPC]
    public void LoadSymbols()
    {

       
        // attach code and material to objects
        for (int i = 0; i < GameEnvironment.Singleton.GetNumOfSymbols; i++)
        {
            // find the colour of the shelf based on the parent tag
            string colour = GameEnvironment.Singleton.Symbols[i].transform.parent.tag;
            shelfColour = ((Material)Resources.Load($"Materials/Shelf/{colour}", typeof(Material)));
            string name = GameEnvironment.Singleton.Materials[i].name;
            //shelfColour = GameEnvironment.Singleton.Symbols[i].transform.parent.GetComponent<Renderer>().material;
            SymbolObject symbol = new SymbolObject(name, GameEnvironment.Singleton.Symbols[i],
                GameEnvironment.Singleton.Materials[i],
                shelfColour,
                GameEnvironment.Singleton.Code[i]);

            Debug.Log(GameEnvironment.Singleton.Code[i]);

            // apply the material onto the symbols object
            GameEnvironment.Singleton.Symbols[i].GetComponent<Renderer>().material = GameEnvironment.Singleton.Materials[i];
            //Debug.Log(shelfColour.ToString());

            testSymbol.Add(symbol);
            Debug.Log("FROM TEST " + testSymbol[0].m_code);
        }

        int symbol1 = Random.Range(0, GameEnvironment.Singleton.GetNumOfSymbols - 1);

        GameEnvironment.Singleton.symbolIndex.Add(symbol1);

        GameEnvironment.AddSymbol(testSymbol[symbol1]);

        Debug.Log(SpawnSymbol.testSymbol[GameEnvironment.Singleton.currentSymbolIndex].m_name);
    }

    // Update is called once per frame
    public static void LoadImage(string symbolPath)
    {
        GameObject screen = GameObject.Find("DownloadImage");

        //Debug.Log(screen.name);

        Sprite sprite = (Sprite)Resources.Load(symbolPath, typeof(Sprite));

        Debug.Log(sprite.name);

        screen.GetComponent<Image>().sprite = sprite;
    }
    public static SymbolObject FindSymbol(string name)
    {
        foreach (var t in testSymbol)
        {
            if (t.m_symbol.name == name)
            {
                return t;
            }
        }

        return null;
    }
}


/*
public class SymbolObject
{
    public string m_name;
    public GameObject m_symbol;
    public Material m_material;
    public Material m_shelfColour;
    public string m_code;
    public string m_fileName;

    public SymbolObject(string name, GameObject symbol, Material material, Material shelfColour, string code, string fileName = "")
    {
        m_name = name;
        m_symbol = symbol;
        m_material = material;
        m_shelfColour = shelfColour;
        m_code = code;
        m_fileName = fileName;
    }
}
*/
