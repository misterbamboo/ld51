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

    public delegate void MinutePassed();
    public event MinutePassed OnMinutePassed;

    public event Action On5MinutesPassed;

    public delegate void HourPassed();
    public event HourPassed OnHourPassed;

    public delegate void TenSecondsPassed();
    public event TenSecondsPassed OnTenSecondsPassed;


    private int secondsPassed = 0;

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
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            secondsPassed++;

            HandleSecond();

            HandleTenSeconds();

            HandleMinute();

            Handle5Minutes();

            HandleHour();
        }
    }

    private void HandleSecond()
    {
        if (OnSecondPassed != null)
        {
            OnSecondPassed();
        }
    }

    private void HandleMinute()
    {
        if (OnMinutePassed == null)
        {
            return;
        }

        var nbSecondInOneMinute = 60;
        var oneMinuteHavePassed = secondsPassed % nbSecondInOneMinute == 0;
        if (oneMinuteHavePassed)
        {
            OnMinutePassed();
        }
    }

    private void Handle5Minutes()
    {
        if (On5MinutesPassed == null)
        {
            return;
        }

        var nbSecondIn5Minutes = 300;
        var fiveMinutesHavePassed = secondsPassed % nbSecondIn5Minutes == 0;
        if (fiveMinutesHavePassed)
        {
            On5MinutesPassed();
        }
    }

    private void HandleHour()
    {
        if(OnHourPassed == null)
        {
            return;
        }

        var nbSecondInOneHour = 3600;
        var oneHourHavePassed = secondsPassed % nbSecondInOneHour == 0;
        if (oneHourHavePassed)
        {
            OnHourPassed();
        }
    }

    private void HandleTenSeconds()
    {
        if(OnTenSecondsPassed == null)
        {
            return;
        }

        var tenSecondsHavePassed = secondsPassed % 10 == 0;
        if (tenSecondsHavePassed)
        {
            OnTenSecondsPassed();
            On5MinutesPassed();
        }
    }
}
