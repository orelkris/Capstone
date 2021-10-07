using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to hold global variables between scenes
public class GameStateController : MonoBehaviour
{
    //value to see if player is player one or two
    public static bool isPlayerOne;
    public static bool isDevBuild = true;
    public static int levelSelect = 1;
}
