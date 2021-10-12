using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{

    GameObject pointerSeconds;
    GameObject pointerMinutes;
    GameObject pointerHours;

    // Start is called before the first frame update
    void Start()
    {
        pointerSeconds = transform.Find("rotation_axis_pointer_seconds").gameObject;
        pointerMinutes = transform.Find("rotation_axis_pointer_minutes").gameObject;
        pointerHours = transform.Find("rotation_axis_pointer_hour").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = 360.0f * Timer.Instance.getTimeLeftPercentage() / 100.0f;

        //pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotation);
        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotation);
        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotation);
    }
}
