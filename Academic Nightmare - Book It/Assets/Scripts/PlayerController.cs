using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    /// <summary>
    /// 
    /// ADDED FOR TEST
    public GameObject book;
    public GameObject player;
    int spawnTreasure = -1;
    int last = 0;
    GameObject temp;
    Text textcode;
    Material shelfColour;

    public static List<SymbolObject> testSymbol = new List<SymbolObject>();
    /// </summary>
    /// 
    public Camera playerCam;
    public GameObject spherePrefab;

    public bool canPlayerOneMove = true;

    float x, y;

    public float movespeed = 5;

    public float sensitivity = 50f;
    private float xRotation;

    //SELECTION MANAGER//
    [SerializeField] private string selectableTag = "Symbol";

    public static bool usePhone = false;

    public Transform _selection;
    public static Text code;

    bool wrongCodeSubmitted;

    //**********************//
    [SerializeField]
    public static InputField inputCode;

    public static Text codePanel;

    Text codeCollector;

    bool showInventory = false;


    ////////////////////////////////////////


    private void Awake()
    {
        
    }
    void Start()
    {

        if (!GameStateController.isPlayerOne)
        {
            textcode = GameObject.Find("Code").GetComponent<Text>();
            
        }
        

        //start following player 2 using the minimap
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        


        if (!GameStateController.isPlayerOne)
        {
            code = GameObject.Find("Code").GetComponent<Text>();
            Debug.Log("Hello from player 2");
        }


        playerCam = Camera.main;

        if (photonView.IsMine)
        {
            playerCam.transform.position = this.transform.position + new Vector3(0, 1.3f, 0);
        }
    }

    private void FixedUpdate()
    {

        if (photonView.IsMine)
        {   
            if(!GameStateController.isPlayerOne)
            {
                Movement();
            }
            else
            {
                if (canPlayerOneMove)
                {
                    Movement();
                }
            }
        }
    }
    private void Update()
    {
        //Show/Hide inventory
        if (GameStateController.isPlayerOne)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showInventory = !showInventory;

                if (showInventory)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    canPlayerOneMove = false;

                    
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    canPlayerOneMove = true;
                }
            }
        }

        if (photonView.IsMine)
        {

            myInput();
            Look();

        }

        if(!GameStateController.isPlayerOne)
        {
            if (_selection != null)
            {
                GameObject.Find("Top").GetComponent<Image>().color = Color.white;
                GameObject.Find("Bottom").GetComponent<Image>().color = Color.white;
                GameObject.Find("Left").GetComponent<Image>().color = Color.white;
                GameObject.Find("Right").GetComponent<Image>().color = Color.white;

                _selection = null;

            }
            RaycastHit hit;

            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

            if (Physics.Raycast(ray, out hit, 5))
            {
                var selection = hit.transform;
                if (selection.CompareTag("Symbol"))
                {

                    GameObject.Find("Top").GetComponent<Image>().color = Color.red;
                    GameObject.Find("Bottom").GetComponent<Image>().color = Color.red;
                    GameObject.Find("Left").GetComponent<Image>().color = Color.red;
                    GameObject.Find("Right").GetComponent<Image>().color = Color.red;

                    if (Input.GetMouseButtonDown(0))
                    {
                        /*
                        code.text = (SpawnSymbol.FindSymbol(hit.collider.gameObject.name).m_code == null ?
                            hit.transform.gameObject.GetComponent<SymbolInformation>().selfCode :
                            SpawnSymbol.FindSymbol(hit.collider.gameObject.name).m_code);
                        */
                        if(photonView.IsMine)
                        {
                            code.text = hit.transform.gameObject.GetComponent<SymbolInformation>().selfObject.m_code;
                        }

                        //if(SpawnBook.FindSymbol(hit.collider.gameObject.name) != null)

                    }

                    _selection = selection;
                }
            }
        }
    }

    private void myInput()
    {
        y = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
    }

    private void Movement()
    {
        this.transform.Translate(x * movespeed * Time.deltaTime, 0, y * movespeed * Time.deltaTime);
        playerCam.transform.position = this.transform.position + new Vector3(0, .85f, 0);
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        float desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        this.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
}

