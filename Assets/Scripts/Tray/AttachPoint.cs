// using UnityEngine;
//
// public class InteractableAttachPoint : InteractableObject
// {
//     public TrayController trayController;
//     public bool isLeft;
//
//     public override void InteractWithHand(Transform handBone, HandMovement hand)
//     {
//         if (!canPickup || trayController == null) return;
//
//         DisableOutline(); 
//         hand.SetTargetCurrentObject(this);
//         canPickup = false;
//
//         trayController.AttachHand(hand, isLeft);
//     }
//
//     public override void StopInteractWithHand(HandMovement hand)
//     {
//         if (trayController == null) return;
//         canPickup = true;
//         trayController.DetachHand(hand, isLeft);
//     }
// }