using UnityEngine;

public class FollowOffset : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }
}
