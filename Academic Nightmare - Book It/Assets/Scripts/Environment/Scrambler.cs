using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scrambler : MonoBehaviour
{
    int[] codeToEscape = { 9, 3, 8 };
    public Text code;
    bool solved = false;

    private void Awake()
    {
        solved = false;
        code = this.GetComponent<Text>();
    }

    void Update()
    {        
        if(this.tag == "Code1")
        {
            if(GameController.symbolsFound == 1)
            {
                code.text = codeToEscape[0].ToString();
                this.GetComponentInParent<Image>().color = Color.green;
                solved = true;
            }
        }
        else if (this.tag == "Code2")
        {
            if (GameController.symbolsFound == 2)
            {
                // fix code
                code.text = codeToEscape[1].ToString();
                this.GetComponentInParent<Image>().color = Color.green;
                solved = true;
            }
        }
        else if (this.tag == "Code3")
        {
            if (GameController.symbolsFound == 3)
            {
                // fix code
                code.text = codeToEscape[2].ToString();
                this.GetComponentInParent<Image>().color = Color.green;
                solved = true;
            }
        }
        
        if(!solved)
        {
            code.text = RandomNumGenerator().ToString();
        }
    }


    [PunRPC]
    private int RandomNumGenerator()
    {
        int output = 0;
                
        output = (Random.Range(0, 10));

            
        return output;
    }
}
