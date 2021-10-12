using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform hotSpot;
    public Transform player1;
    public Camera camera;
    Vector3 dir;
    public Slider closer;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("PlayerOne(Clone)") != null || GameObject.Find("Player(Clone)") != null)
        {
            player1 = GameObject.Find("PlayerOne(Clone)").transform;
            camera = GameObject.Find("PlayerOne(Clone)").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if(GameObject.Find("PlayerOne(Clone)") != null || GameObject.Find("PlayerOne(Clone)") != null)
        {

            Vector3 toPosition = (hotSpot.position-player1.position);
            float angleToPosition = Vector3.Angle(toPosition, player1.forward);
            float angle = Vector3.SignedAngle(hotSpot.position, player1.forward, Vector3.up); //Returns the angle between -180 and 180.

            //Debug.Log(Vector3.AngleBetween(player1.forward, toPosition));

            if(Vector3.AngleBetween(player1.right, toPosition) <= 1.3 && Vector3.AngleBetween(player1.right, toPosition) >= 0)
            {
                Debug.Log(toPosition.magnitude);
                if(toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.forward, toPosition) * 0); 
                }
                else
                {
                    //Debug.Log("Left");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * -10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.forward, toPosition));
                }

            }
            else if (Vector3.AngleBetween(player1.right, toPosition) <= 3 && Vector3.AngleBetween(player1.right, toPosition) > 1.3)
            {
                if (toPosition.magnitude < 3)
                {
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, -Vector3.AngleBetween(player1.forward, toPosition) * 0);
                }
                else
                {
                    //Debug.Log("Right");
                    // Debug.Log(Vector3.AngleBetween(player1.forward, toPosition) * 10);
                    this.transform.rotation = Quaternion.EulerAngles(0, 0, Vector3.AngleBetween(player1.forward, toPosition));
                    //this.transform.Rotate(0,0, Vector3.AngleBetween(player1.forward, toPosition));
                }

            }

            closer.value = 1 / (hotSpot.position - player1.position).magnitude * 3f;
        }    
    }
}
