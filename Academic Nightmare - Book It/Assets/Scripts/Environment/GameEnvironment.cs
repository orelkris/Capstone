using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameEnvironment
{
    public struct coordinates
    {
        public float x;
        public float y;
        public float z;
    };

    private static GameEnvironment instance;

    private List<GameObject> checkpoints = new List<GameObject>();

    private List<GameObject> shelves = new List<GameObject>();

    private List<GameObject> symbols = new List<GameObject>();

    public List<GameObject> Checkpoints { get { return checkpoints; } }

    public List<Material> Materials = new List<Material>();

    public List<string> Code = new List<string>();

    public List<GameObject> Shelves { get { return shelves; } }

    public List<GameObject> Symbols { get { return symbols; } }

    public List<SymbolObject> Symbol { get; set; }

    public List<coordinates> HotSpotLocation = new List<coordinates>();

    public List<int> symbolIndex { get; set; }

    public int currentTreasureIndex;


    public static GameEnvironment Singleton
    {


        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                //instance.Checkpoints.AddRange(
                  //  GameObject.FindGameObjectsWithTag("Checkpoint"));

                //instance.Shelves.AddRange(
                   // GameObject.FindGameObjectsWithTag("Shelf"));

                instance.Symbols.AddRange(
                    GameObject.FindGameObjectsWithTag("Symbol"));

                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();

                for (int i = 1; i <= 4; i++)
                {
                    instance.Materials.Add((Material)Resources.Load($"Materials/Symbol{i}", typeof(Material)));

                }

                for (int i = 1; i <= 4; i++)
                {
                    instance.Code.Add(RandomNameGenerator());
                }

                Shuffle(instance.Materials);

                instance.Symbol = new List<SymbolObject>();

                instance.symbolIndex = new List<int>();

                instance.currentTreasureIndex = 0;
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
        if (GameEnvironment.Singleton.Symbol[GameEnvironment.Singleton.currentTreasureIndex].m_code == symbol.m_code)
        {
            return true;
        }

        return false;
    }

    public static SymbolObject CurrentSymbol()
    {
        return GameEnvironment.Singleton.Symbol[GameEnvironment.Singleton.currentTreasureIndex];
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