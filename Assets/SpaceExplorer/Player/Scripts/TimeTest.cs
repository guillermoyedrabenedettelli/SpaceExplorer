using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    private float waitTime = 0.1f;
    private float timer = 0.0f;
    bool change_Time=false;

    void Awake()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (!change_Time)
        {
            if (timer > 0.1f)
            {
                Debug.Log(timer);
                // Remove the recorded 2 seconds.
                timer = timer - waitTime;
                change_Time = true;
            }
        }

        if (change_Time)
        {
            if (timer > 0.2f)
            {
                Debug.Log(timer);
                // Remove the recorded 2 seconds.
                timer = timer - waitTime;
                change_Time = false;
            }
        }
    }
}
