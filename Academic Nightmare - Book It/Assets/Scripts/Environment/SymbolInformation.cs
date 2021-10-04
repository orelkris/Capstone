using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInformation : MonoBehaviour
{
    public SymbolObject selfObject;
    public string selfCode;
    public Material selfMaterial;
    public Material shelfColour;
    public string selfName;



    // Start is called before the first frame update
    void Start()
    {

        string code = RandomNameGenerator();
        PhotonView pv = GetComponent<PhotonView>();
        pv.RPC(nameof(ReceiveCode), RpcTarget.All, code);

         
        selfMaterial = this.GetComponent<Renderer>().material;
        int index = selfMaterial.name.IndexOf(' ');
        selfName = selfMaterial.name.Substring(0, index);
        shelfColour = GameController.FindColour(this.GetComponent<Transform>().position);
        Debug.Log("FOUND SHELF COLOUR FROM SYMBOL " + shelfColour);

        selfObject = new SymbolObject(selfName, selfMaterial, shelfColour, selfCode);
        Debug.Log("FINAL CODE : " + selfObject.m_code);

    }


    [PunRPC]
    public string GetObjectCode()
    {
        return this.selfObject.m_code;
    }
    
    [PunRPC]
    private string RandomNameGenerator()
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

    [PunRPC]
    private void ReceiveCode(string code)
    {
        selfCode = code;
    }



}

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

    public SymbolObject(string name,  Material material, string code)
    {
        m_name = name;
        m_material = material;
        m_code = code;
    }

    public SymbolObject(string name, Material material, Material shelfColour, string code)
    {
        m_name = name;
        m_material = material;
        m_shelfColour = shelfColour;
        m_code = code;
    }


    public void SetShelfColour(Material newColour)
    {
        m_shelfColour = newColour;
    }

    public string GetShelfColour()
    {
        return this.m_shelfColour.name;
    }
}

