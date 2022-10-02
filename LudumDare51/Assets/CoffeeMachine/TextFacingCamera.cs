using UnityEngine;

public class TextFacingCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main?.transform != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
