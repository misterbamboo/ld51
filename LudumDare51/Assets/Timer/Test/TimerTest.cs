using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<Timer>().OnTimerDone += TimerDone;
        GameObject.FindObjectOfType<Timer>().OnSecondPassed += SecondPassed;
    }

    void SecondPassed()
    {
        Debug.Log("Second passed");
    }

    void TimerDone()
    {
        Debug.Log($"Timer is done! {System.DateTime.Now.ToString()}");
    }
}
