using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollower : MonoBehaviour
{
    public Transform player2;
    bool foundPlayerTwo = false;
    void Start()
    {
        
        if(GameObject.Find("PlayerTwo(Clone)") != null)
        {
            player2 = GameObject.Find("PlayerTwo(Clone)").transform;
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameStateController.isPlayerOne)
        {
            if(GameObject.Find("PlayerTwo(Clone)") != null)
            {
                if (!foundPlayerTwo)
                {
                    player2 = GameObject.Find("PlayerTwo(Clone)").transform;
                    foundPlayerTwo = true;
                }

                Vector3 newPosition = player2.position;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }

            
        }
    }
}
