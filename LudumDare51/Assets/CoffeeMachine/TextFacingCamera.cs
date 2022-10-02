using UnityEngine;

public class TextFacingCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.current?.transform != null)
        {
            transform.LookAt(Camera.current.transform);
            transform.rotation = Quaternion.LookRotation(Camera.current.transform.forward);
        }
    }
}
