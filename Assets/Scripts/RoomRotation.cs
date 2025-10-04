using UnityEngine;

public class RoomRotation : MonoBehaviour
{
    [SerializeField] private Transform forearmBone; // forearm being copied
    [SerializeField] private Transform upperarmBone; // upperarm being copied
    [SerializeField] private Transform wristBone; // upperarm being copied

    [SerializeField] private Transform forearmCopier; // copied forearm
    [SerializeField] private Transform upperarmCopier; // copied upperarm
    [SerializeField] private Transform wristCopier; // upperarm being copied

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

        if (wristBone != null)
        {
            // Copy world rotation
            wristCopier.rotation = wristBone.rotation;
        }

        ogPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 movement = robotHandMovement.movement;
        movement.x *= -1.0f;
        transform.localPosition = movement * speed + ogPosition;

        if (wristBone != null)
        {
            //Quaternion relativeRotation = Quaternion.Euler(robotHandMovement.wristRotateY, robotHandMovement.wristRotateX, 0); ;
            //if (!robotHandMovement.left)
            //{
            //    //relativeRotation.y *= -1.0f;
            //}
            wristCopier.localRotation = wristBone.localRotation;
        }
    }
}
