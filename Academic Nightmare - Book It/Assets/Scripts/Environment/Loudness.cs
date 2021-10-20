using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loudness : MonoBehaviour
{
    double radius;

    RectTransform detector;
    // Start is called before the first frame update
    void Start()
    {
        detector = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        detector.sizeDelta = new Vector2(1 * PushToTalk.currentPeak * 100, PushToTalk.currentPeak * 100);
    }
}
