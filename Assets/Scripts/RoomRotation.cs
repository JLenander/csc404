using UnityEngine;
using UnityEngine.Serialization;

public class RoomRotation : MonoBehaviour
{
    [SerializeField] private Transform forearmBone; // forearm being copied from
    [SerializeField] private Transform upperarmBone; // upperarm being copied from
    [FormerlySerializedAs("wristBone")] [SerializeField] private Transform wristRollSource; // wrist roll rotation being copied from
    [SerializeField] private Transform wristPitchYawSource; // wrist pitch / yaw rotation being copied from
    
    [SerializeField] private Transform forearmCopier; // copied to forearm
    [SerializeField] private Transform upperarmCopier; // copied to upperarm
    [SerializeField] private Transform wristCopier; // copied to wrist

    [SerializeField] private Transform robotTarget;
    [SerializeField] private float speed;
    private HandMovement robotHandMovement;
    private Vector3 ogPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        robotHandMovement = robotTarget.GetComponent<HandMovement>();

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
        
        ogPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 movement = robotHandMovement.movement;
        movement.x *= -1.0f;
        transform.localPosition = movement * speed + ogPosition;

        if (wristRollSource != null &&  wristPitchYawSource != null)
        {
            // Combine the rotations of the wrist roll rotation source and the wrist pitch/yaw rotation source with some quaternion math
            // Need to flip the yaw and roll rotations.
            var rollRotation = Quaternion.Inverse(wristRollSource.localRotation);
            var pitchYawRotation = wristPitchYawSource.localRotation;
            pitchYawRotation.y = -1 * pitchYawRotation.y;
            pitchYawRotation.z = -1 * pitchYawRotation.z;
            wristCopier.localRotation = pitchYawRotation * rollRotation;
        }
    }
}
