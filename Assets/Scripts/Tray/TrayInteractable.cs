// using UnityEngine;
//
// public class TrayInteractable : InteractableObject
// {
//     private TrayController trayController;
//     private Transform leftHandAttached;
//     private Transform rightHandAttached;
//
//     private void Awake()
//     {
//         trayController = GetComponent<TrayController>();
//     }
//
//     public override void InteractWithHand(Transform wristBone, HandMovement hand)
//     {
//         base.InteractWithHand(wristBone, hand);
//
//         // Determine which side to attach based on proximity
//         float leftDist = Vector3.Distance(wristBone.position, trayController.leftAttachPoint.position);
//         float rightDist = Vector3.Distance(wristBone.position, trayController.rightAttachPoint.position);
//
//         if (leftDist < rightDist && leftHandAttached == null)
//         {
//             leftHandAttached = wristBone;
//             trayController.AttachHand(wristBone, true);
//         }
//         else if (rightHandAttached == null)
//         {
//             rightHandAttached = wristBone;
//             trayController.AttachHand(wristBone, false);
//         }
//     }
//
//     public override void StopInteractWithHand(Transform wristBone, HandMovement hand)
//     {
//         base.StopInteractWithHand(wristBone, hand);
//
//         if (wristBone == leftHandAttached)
//         {
//             leftHandAttached = null;
//             trayController.DetachHand(true);
//         }
//         else if (wristBone == rightHandAttached)
//         {
//             rightHandAttached = null;
//             trayController.DetachHand(false);
//         }
//     }
// }