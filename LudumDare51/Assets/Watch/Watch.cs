using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    [SerializeField]
    private GameObject secondHand;

    [SerializeField]
    private GameObject minuteHand;
        
    [SerializeField]
    private GameObject hourHand;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Timer.Instance.OnSecondPassed += RotateSecondHand;   
        Timer.Instance.OnMinutePassed += RotateMinuteHand;
        Timer.Instance.OnHourPassed += RotateHourHand;
        Timer.Instance.OnTenSecondsPassed += StartAnim;

        animator = GetComponent<Animator>();
    }

    void RotateSecondHand()
    {
        secondHand.transform.Rotate(0, 0, -6);
    }

    void RotateMinuteHand()
    {
        minuteHand.transform.Rotate(0, 0, -6);
    }

    void RotateHourHand()
    {
        hourHand.transform.Rotate(0, 0, -6);
    }

    void StartAnim()
    {
        animator.SetBool("Play", true);
        StartCoroutine(StopAnim());
    }

    // Coroutine that wait two real seconds and then set the animation to false
    IEnumerator StopAnim()
    {
        yield return new WaitForSecondsRealtime(2);
        animator.SetBool("Play", false);
    }
}
