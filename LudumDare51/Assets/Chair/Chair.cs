using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField]
    BoxCollider collider;

    [SerializeField]
    Transform target;

    [SerializeField]
    Transform chairTransform;

    [SerializeField]
    Transform characterTransform;

    [SerializeField]
    float timerToPushChair = 30f;

    [SerializeField]
    float timerToBringBackChair = 30f; 

    [SerializeField]
    float chairTimeToMove = 2f;

    [SerializeField]
    float characterTimeToMove = 2f;

    private Vector3 chairOriginalPosition;

    private Vector3 characterOriginalPosition;

    void Awake()
    {
        chairOriginalPosition = chairTransform.position;
        characterOriginalPosition = characterTransform.position;

        collider.gameObject.SetActive(false);

        StartCoroutine(CountDownToMoveChair());
    }

    IEnumerator CountDownToMoveChair()
    {
        yield return new WaitForSecondsRealtime(timerToPushChair);
        MoveChair();
        BringBackTheChairAfterTime();
    }

    public void MoveChair()
    {
        Vector3 targetPosition = new Vector3(target.position.x, chairTransform.position.y, target.position.z);
        collider.gameObject.SetActive(true);
        
        var characterGroundPos = new Vector3(characterTransform.position.x, 0.5f, characterTransform.position.z);
        StartCoroutine(LerpCharacterToTarget(characterGroundPos));

        StartCoroutine(LerpChairTowardPosition(targetPosition, chairTimeToMove));
    }

    IEnumerator LerpChairTowardPosition(Vector3 endValue, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            chairTransform.position = Vector3.Lerp(chairTransform.position, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        chairTransform.position = endValue;
    }

    IEnumerator LerpCharacterToTarget(Vector3 targetPos)
    {
        float time = 0;
        while (time < characterTimeToMove)
        {
            characterTransform.position = Vector3.Lerp(characterTransform.position, targetPos, time / characterTimeToMove);
            time += Time.deltaTime;
            yield return null;
        }
        characterTransform.position = targetPos;
    }

    void BringBackTheChairAfterTime()
    {
        StartCoroutine(CountDownToBringBackChair());
    }

    IEnumerator CountDownToBringBackChair()
    {
        yield return new WaitForSecondsRealtime(timerToBringBackChair);
        collider.gameObject.SetActive(false);
        StartCoroutine(LerpChairTowardPosition(chairOriginalPosition, chairTimeToMove));

        StartCoroutine(LerpCharacterToTarget(characterOriginalPosition));

        StartCoroutine(CountDownToMoveChair());
    }
}
