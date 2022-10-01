using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFacingCamera : MonoBehaviour
{
    private Camera cameraToFaceAt;

    void Start()
    {
        cameraToFaceAt = Camera.main;
    }

    void Update()
    {
        transform.LookAt(cameraToFaceAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToFaceAt.transform.forward);
    }
}
