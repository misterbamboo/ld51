using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    int secondsPassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<Timer>().OnTenSecondsPassed += TimerDone;
        GameObject.FindObjectOfType<Timer>().OnSecondPassed += SecondPassed;
    }

    void SecondPassed()
    {
        secondsPassed++;
        // Debug.Log($"{secondsPassed} Second passed");
    }

    void TimerDone()
    {
        Debug.Log("10 seconds passed");
    }
}
