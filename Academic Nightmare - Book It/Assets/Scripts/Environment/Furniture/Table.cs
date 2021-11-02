using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Table : MonoBehaviour
{
    BoxCollider bCollider;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
    }


}
