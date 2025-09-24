using UnityEngine;

public class RoomRotation : MonoBehaviour
{
    [SerializeField] private Transform forearmBone; // forearm being copied
    [SerializeField] private Transform upperarmBone; // upperarm being copied
    [SerializeField] private Transform wristBone; // upperarm being copied

    [SerializeField] private Transform forearmCopier; // copied forearm
    [SerializeField] private Transform upperarmCopier; // copied upperarm
    [SerializeField] private Transform wristCopier; // upperarm being copied

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void LateUpdate()
    {
        if (forearmBone != null)
        {
            // Copy world rotation
            forearmCopier.rotation = forearmBone.rotation;
        }

        if (upperarmBone != null)
        {
            // Copy world rotation
            upperarmCopier.rotation = upperarmBone.rotation;
        }

        if (wristBone != null)
        {
            // Copy world rotation
            wristCopier.rotation = wristBone.rotation;
        }
    }
}
