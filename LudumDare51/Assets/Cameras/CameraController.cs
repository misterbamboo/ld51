using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
}
