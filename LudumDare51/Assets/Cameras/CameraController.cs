using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Timer timer;
    [SerializeField] Boss boss;
    [SerializeField] Watch watch;
    [SerializeField] Transform bossLosePosition;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera winCamera;
    [SerializeField] Camera loseCamera;

    [SerializeField] AngryBar angryBar;

    void Start()
    {
        winCamera.gameObject.SetActive(false);
        loseCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        angryBar.OnAngryBarFull += AngryBar_OnAngryBarFull;
        timer.On5MinutesPassed += Timer_On5MinutesPassed;
    }

    private void Timer_On5MinutesPassed()
    {
        Win();
    }

    private void AngryBar_OnAngryBarFull()
    {
        Lose();
    }

    private void Lose()
    {
        var followPath = boss.GetComponent<NPCFollowPath>();
        followPath.enabled = false;

        boss.transform.position = bossLosePosition.transform.position;
        boss.transform.rotation = bossLosePosition.transform.rotation;

        watch.gameObject.SetActive(false);

        mainCamera.gameObject.SetActive(false);
        loseCamera.gameObject.SetActive(true);
    }

    private void Win()
    {
        watch.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        winCamera.gameObject.SetActive(true);
    }
}
