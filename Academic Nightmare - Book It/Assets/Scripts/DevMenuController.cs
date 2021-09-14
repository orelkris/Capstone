using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenuController : MonoBehaviour
{
    
    //Get into game quickly as player one
    public void StartTestAsPlayerOne()
    {
        NetworkController.Instance.SetPlayerOne();
        NetworkController.Instance.DevTestCreateJoinRoom();
    }

    //Get into game quickly as player two
    public void StartTestAsPlayerTwo()
    {
        NetworkController.Instance.SetPlayerTwo();
        NetworkController.Instance.DevTestCreateJoinRoom();
    }
}
