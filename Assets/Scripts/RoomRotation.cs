using UnityEngine;

public class RoomRotation : MonoBehaviour
{
    [SerializeField] private Transform forearmBone;
    [SerializeField] private Transform upperarmBone;

    [SerializeField] private Transform forearmCorridor;
    [SerializeField] private Transform upperarmCorridor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void LateUpdate()
    {
        if (forearmBone != null)
        {
            // Copy world rotation
            forearmCorridor.rotation = forearmBone.rotation;
        }

        if (upperarmBone != null)
        {
            // Copy world rotation
            upperarmCorridor.rotation = upperarmBone.rotation;
        }
    }
}
