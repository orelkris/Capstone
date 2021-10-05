//You are free to use this script in Free or Commercial projects
//sharpcoderblog.com @2019

using UnityEngine;

public class SC_InventorySystem : MonoBehaviour
{
    public Texture crosshairTexture;
    public PlayerController playerController;
    public SC_PickItem[] availableItems; //List with Prefabs of all the available items

    //button texture
    public Texture buttonMinimap;
    public Texture Minimap;

    //Available items slots
    int[] itemSlots = new int[12];
    bool showInventory = false;
    float windowAnimation = 1;
    float animationTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Initialize Item Slots
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = -1;
        }

        playerController = GameObject.Find("PlayerOne(Clone)").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Show/Hide inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory = !showInventory;
            animationTimer = 0;

            if (showInventory)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (animationTimer < 1)
        {
            animationTimer += Time.deltaTime;
        }

        if (showInventory)
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 0, animationTimer);
            PlayerController.canPlayerOneMove = false;
        }
        else
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 1f, animationTimer);
            PlayerController.canPlayerOneMove = true;
        }

        
    }

    void FixedUpdate()
    {
        //Detect if the Player is looking at any item
        RaycastHit hit;
        Ray ray = playerController.playerCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));///TH

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            Transform objectHit = hit.transform;
            if(objectHit == GameObject.Find("MinimapButton"))
            {
                Debug.Log("MINIMAP PRESSED");
            }

            
        }
        else
        {
           
        }
    }

    void OnGUI()
    {
        //Inventory UI
        GUI.Label(new Rect(5, 5, 200, 25), "Press 'Tab' to open Inventory");

        //Inventory window
        if (windowAnimation < 1)
        {
            GUILayout.BeginArea(new Rect(10 - (430 * windowAnimation), Screen.height / 2 - 200, 302, 430), GUI.skin.GetStyle("box"));

            GUILayout.Label("Inventory", GUILayout.Height(25));

            GUILayout.BeginVertical();
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    GUILayout.Button(buttonMinimap);

                    //Empty slot
                    GUILayout.Box("", GUILayout.Width(5), GUILayout.Height(5));
                }
                //GUILayout.BeginHorizontal();
                //Display 3 items in a row


                        //Detect if the mouse cursor is hovering over item
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        Vector2 eventMousePositon = Event.current.mousePosition;
                        if (Event.current.type == EventType.Repaint && lastRect.Contains(eventMousePositon))
                        {

                            GUILayout.Box(Minimap);

                            GUILayout.Box("", GUILayout.Width(20), GUILayout.Height(20));
                        }

                        GUI.enabled = true;
                    
                
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                
            }

            GUILayout.EndArea();
        }


       
    }
}