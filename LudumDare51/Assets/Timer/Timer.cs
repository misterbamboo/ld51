using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // this class is a singleton
    public static Timer Instance { get; private set; }

    public delegate void SecondPassed();
    public event SecondPassed OnSecondPassed;

    public delegate void TimerDone();
    public event TimerDone OnTimerDone;

    // instatiate the singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        int time = 10;
        while (time > 0)
        {
            OnSecondPassed();
            yield return new WaitForSeconds(1);
            time--;
        }
        
        OnTimerDone();
        StartCoroutine(CountDown());
    }
}
