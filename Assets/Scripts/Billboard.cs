using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Make the object face the camera always, not affected by character rotation
    void LateUpdate()
    {
        var cam = Camera.main;
        if (cam)
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
    }
}
