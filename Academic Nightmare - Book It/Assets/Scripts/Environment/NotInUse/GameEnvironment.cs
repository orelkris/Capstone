using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

[PunRPC]
public class GameEnvironment : MonoBehaviour
{
    private const int numOfSymbols = 3;

    public int GetNumOfSymbols { get { return numOfSymbols; } }
    public struct coordinates
    {
        public float x;
        public float y;
        public float z;
    };

    bool runOnce = false;
    private static GameEnvironment instance;

    private List<GameObject> checkpoints = new List<GameObject>();

    private List<GameObject> shelves = new List<GameObject>();

    private List<GameObject> symbols = new List<GameObject>();

    public List<GameObject> Checkpoints { get { return checkpoints; } }

    public List<Material> Materials = new List<Material>();

    //Hacker screen list of symbols
    public List<Image> ScreenSymbols = new List<Image>();
    public List<Sprite> SpriteSymbols = new List<Sprite>();
    private List<Button> symbolButtons = new List<Button>();

    public List<string> Code = new List<string>();

    public List<GameObject> Shelves { get { return shelves; } }

    public List<GameObject> Symbols { get { return symbols; } }

    public List<SymbolObject> Symbol { get; set; }

    public List<coordinates> HotSpotLocation = new List<coordinates>();

    public List<int> symbolIndex { get; set; }

    public int currentSymbolIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static GameEnvironment Singleton
    {
        get
        {
            if(!instance.runOnce)
            {
                //instance.Checkpoints.AddRange(
                //  GameObject.FindGameObjectsWithTag("Checkpoint"));

                //instance.Shelves.AddRange(
                // GameObject.FindGameObjectsWithTag("Shelf"));

                instance.Symbols.AddRange(
                    GameObject.FindGameObjectsWithTag("Symbol"));

                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();

                GameObject canvasGlobal = GameObject.Find("CanvasGlobal");

                // this will select the symbol materials and keep track of them to be randomly applied
                // to the symbol objects in the scene through SpawnSymbol.cs 
                for (int i = 0; i < numOfSymbols; i++)
                {
                    instance.Materials.Add((Material)Resources.Load($"Materials/Symbol{i + 1}", typeof(Material)));
                }

                // this will generate a list of 3 letter codes that will be attached to symbol objects
                // once they are spawned through the SpawnSymbol.cs class
                for (int i = 0; i < numOfSymbols; i++)
                {
                    instance.Code.Add(RandomNameGenerator());
                }

                // shuffle the materials 
                Shuffle(instance.Materials);

                instance.Symbol = new List<SymbolObject>();

                instance.symbolIndex = new List<int>();

                instance.currentSymbolIndex = 0;

                instance.runOnce = true;
            }
               
            

            return instance;
        }
    }

    static string RandomNameGenerator()
    {
        string output = "";
        char c;

        for (int i = 0; i < 3; i++)
        {
            // store a random char in c
            c = (char)('A' + Random.Range(0, 25));

            // concat the random char into an output string
            output += c;
        }

        return output;
    }

    public static List<T> Shuffle<T>(List<T> list)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }


    public static bool CorrectPick(SymbolObject symbol)
    {
        if (GameEnvironment.Singleton.Symbol[GameEnvironment.Singleton.currentSymbolIndex].m_code == symbol.m_code)
        {
            return true;
        }

        return false;
    }

    public static SymbolObject CurrentSymbol()
    {
        
        return GameEnvironment.Singleton.Symbol[GameEnvironment.Singleton.currentSymbolIndex];
    }
    public static bool ContainsSymbol(SymbolObject symbol)
    {
        foreach (var t in instance.Symbol)
        {
            if (t.m_symbol.name == symbol.m_symbol.name)
            {
                return true;
            }
        }

        return false;
    }

    public static void AddSymbol(SymbolObject symbol)
    {
        instance.Symbol.Add(symbol);
    }

}