using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    public Camera playerCam;
    public GameObject spherePrefab;

    float x, y;

    public float movespeed = 5;

    public float sensitivity = 50f;
    private float xRotation;

    private void Awake()
    {

    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCam = Camera.main;

        if (photonView.IsMine == true)
        {
            playerCam.transform.position = this.transform.position + new Vector3(0, 1.3f, 0);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == true)
        {
            Movement();
        }
    }

    private void Update()
    {
        if (photonView.IsMine == true)
        {
            myInput();
            Look();
        }
    }

    private void myInput()
    {
        y = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");



        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
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

    private void Shoot()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = playerCam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            photonView.RPC("RPC_Shoot", RpcTarget.All, hit.point);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition)
    {
        Instantiate(spherePrefab, hitPosition, Quaternion.identity);
    }
}
