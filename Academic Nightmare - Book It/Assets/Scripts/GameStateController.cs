using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to hold global variables between scenes
public class GameStateController : MonoBehaviour
{
    //value to see if player is player one or two
    public static bool isPlayerOne = true;
    public static bool isDevBuild = false;
    public static int levelSelect = 1;
    internal static bool isGhost;
}