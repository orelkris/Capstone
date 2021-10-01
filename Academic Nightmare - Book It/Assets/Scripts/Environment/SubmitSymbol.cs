using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitSymbol : MonoBehaviour
{
    public void Submit()
    {
        string name = SpawnSymbol.testSymbol[GameEnvironment.Singleton.currentSymbolIndex].m_name;
        if (this.GetComponent<Image>().sprite.name == name)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Incorrect");
            //Debug.Log(this.GetComponent<Image>().sprite.name);
        }
    }
}
