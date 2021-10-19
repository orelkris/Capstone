using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Compass : MonoBehaviour
{
    public GameObject hotSpot;
    public GameObject player1;
    public Camera camera;
    Vector3 dir;
    public Slider closer;
    // Start is called before the first frame update
    public Vector3 toPosition;
    public Vector3 playerOneForward;

    // Update is called once per frame
    public void Update()
    {
        player1 = GameObject.FindGameObjectWithTag("Hacker");
        hotSpot = GameObject.FindGameObjectWithTag("HotSpot");

        //if (GameObject.Find("PlayerOne(Clone)") != null && !GameStateController.isPlayerOne)
        if (player1 != null && hotSpot != null && !GameStateController.isPlayerOne)
        {
            toPosition = hotSpot.transform.position - player1.transform.position;
            //float angleToPosition = Vector3.Angle(toPosition, playerOneForward);
            //float angle = Vector3.SignedAngle(hotSpot.position, playerOneForward, Vector3.up); //Returns the angle between -180 and 180.


            //Debug.Log(Vector3.AngleBetween(player1.forward, toPosition));

            if (Vector3.AngleBetween(player1.transform.right, toPosition) <= 1.3 && Vector3.AngleBetween(player1.transform.right, toPosition) >= 0)
            {
                Debug.Log(toPosition.magnitude);
                if (toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.transform.forward, toPosition) * 0);
                }
                else
                {
                    //Debug.Log("Left");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * -10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.transform.forward, toPosition));
                }

            }
            else if (Vector3.AngleBetween(player1.transform.right, toPosition) <= 3 && Vector3.AngleBetween(player1.transform.right, toPosition) > 1.3)
            {
                if (toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.transform.forward, toPosition) * 0);
                }
                else
                {
                    //Debug.Log("Right");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * 10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, Vector3.AngleBetween(player1.transform.forward, toPosition));
                    //this.transform.Rotate(0,0, Vector3.AngleBetween(player1.forward, toPosition));
                }

            }
            /*
            Debug.Log(GameObject.Find("PlayerOne(Clone)").transform.position - GameObject.Find("HotSpot(Clone)").transform.position);
            toPosition = GameObject.Find("PlayerOne(Clone)").GetComponent<PlayerController>().toPosition;
            Debug.Log("PLAYER 1 IS MOVING " + toPosition);
            //float angleToPosition = Vector3.Angle(toPosition, playerOneForward);
            //float angle = Vector3.SignedAngle(hotSpot.position, playerOneForward, Vector3.up); //Returns the angle between -180 and 180.


            //Debug.Log(Vector3.AngleBetween(player1.forward, toPosition));

            if (Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.right, toPosition) <= 1.3 && Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.right, toPosition) >= 0)
            {
                Debug.Log(toPosition.magnitude);
                if(toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.forward, toPosition) * 0); 
                }
                else
                {
                    //Debug.Log("Left");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * -10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.forward, toPosition));
                }

            }
            else if (Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.right, toPosition) <= 3 && Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.right, toPosition) > 1.3)
            {
                if (toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.forward, toPosition) * 0);
                }
                else
                {
                    //Debug.Log("Right");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * 10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, Vector3.AngleBetween(GameObject.Find("PlayerOne(Clone)").transform.forward, toPosition));
                    //this.transform.Rotate(0,0, Vector3.AngleBetween(player1.forward, toPosition));
                }

            }
            */

            closer.value = 1 / (toPosition).magnitude * 3f;
        }
    }
}
