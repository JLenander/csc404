// using UnityEngine;
//
// public class TrayController : MonoBehaviour
// {
//     [Header("Setup")]
//     private Rigidbody rb;
//     public Transform leftAttachPoint;
//     public Transform rightAttachPoint;
//     public Transform counterSpawnPoint;  // assign the empty CounterSpawnPoint object
//
//     [Header("Cups")]
//     public GameObject cupPrefab;
//     private GameObject leftCup;
//     private GameObject rightCup;
//     public Vector3 leftCupOffset = new Vector3(2.64f, 2f, 2.68f);
//     public Vector3 rightCupOffset = new Vector3(-0.58f, 2f, 0.4f);
//     public float dropThresholdY = -1.5f;
//
//     [Header("Hand Snapping")]
//     public float snapDistance = 1.5f;
//     public float magnetStrength = 10f;
//
//     [Header("Table Placement")]
//     public LayerMask tableLayer;
//     public float placeDistance = 0.5f;
//     
//     private HandMovement leftHand;
//     private HandMovement rightHand;
//     private bool isActive;
//     private Vector3 originalLeftHandMovement;
//     private Vector3 originalRightHandMovement;
//     
//     private void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.isKinematic = true;
//         SpawnCups();
//     }
//
//     private void Update()
//     {
//         if (isActive)
//         {
//             UpdateTrayPosition();
//             CheckCupDrop();
//             CheckTablePlacement();
//         }
//     }
//
//     public void AttachHand(HandMovement hand, bool left)
//     {
//         if (left)
//         {
//             leftHand = hand;
//             originalLeftHandMovement = hand.movement;
//             // Snap hand to attach point
//             SnapHandToAttachPoint(hand, leftAttachPoint);
//         }
//         else
//         {
//             rightHand = hand;
//             originalRightHandMovement = hand.movement;
//             // Snap hand to attach point
//             SnapHandToAttachPoint(hand, rightAttachPoint);
//         }
//
//         // Activate tray when both hands are attached
//         if (leftHand != null && rightHand != null)
//         {
//             rb.isKinematic = false;
//             isActive = true;
//         }
//     }
//
//     public void DetachHand(HandMovement hand, bool left)
//     {
//         if (left)
//             leftHand = null;
//         else
//             rightHand = null;
//
//         if (leftHand == null && rightHand == null)
//         {
//             rb.isKinematic = true;
//             isActive = false;
//         }
//     }
//
//     private void SnapHandToAttachPoint(HandMovement hand, Transform attachPoint)
//     {
//         // Calculate the offset needed to snap to attach point
//         Vector3 worldAttachPoint = attachPoint.position;
//         Vector3 handWorldPos = hand.transform.position;
//         
//         // Convert to hand's local movement space
//         Vector3 offset = worldAttachPoint - handWorldPos;
//         hand.movement += offset / hand.speed;
//     }
//     
//     private void UpdateTrayPosition()
//     {
//         if (leftHand == null || rightHand == null) return;
//
//         // Calculate tray center from both hands
//         Vector3 leftHandPos = leftHand.transform.position;
//         Vector3 rightHandPos = rightHand.transform.position;
//         Vector3 centerPos = (leftHandPos + rightHandPos) / 2f;
//
//         // Move tray to center position
//         rb.MovePosition(Vector3.Lerp(rb.position, centerPos, Time.deltaTime * 5f));
//
//         // Calculate tilt based on hand height difference
//         float leftY = leftHandPos.y;
//         float rightY = rightHandPos.y;
//         float heightDiff = rightY - leftY;
//         float tiltStrength = 15f;
//
//         // Calculate roll from hand positions
//         Vector3 leftToRight = rightHandPos - leftHandPos;
//         float rollAngle = -heightDiff * tiltStrength;
//         
//         // Calculate pitch from hand Z positions
//         float leftZ = leftHand.transform.position.z;
//         float rightZ = rightHand.transform.position.z;
//         float avgZ = (leftZ + rightZ) / 2f;
//         float pitchAngle = (avgZ - rb.position.z) * tiltStrength;
//
//         Quaternion targetRotation = Quaternion.Euler(pitchAngle, rb.rotation.eulerAngles.y, rollAngle);
//         rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 3f));
//     }
//
//     private void CheckCupDrop()
//     {
//         if (leftCup == null || rightCup == null) return;
//
//         if (leftCup.transform.position.y < dropThresholdY || rightCup.transform.position.y < dropThresholdY)
//         {
//             DestroyTrayAndRespawn();
//         }
//     }
//
//     private void DestroyTrayAndRespawn()
//     {
//         // Detach hands first
//         if (leftHand != null)
//         {
//             leftHand.currObj = null;
//         }
//         if (rightHand != null)
//         {
//             rightHand.currObj = null;
//         }
//
//         Destroy(leftCup);
//         Destroy(rightCup);
//         
//         Instantiate(gameObject, counterSpawnPoint.position, Quaternion.identity);
//         Destroy(gameObject);
//     }
//     
//     void SpawnCups()
//     {
//      leftCup = Instantiate(cupPrefab, transform.position + leftCupOffset, Quaternion.identity, transform);
//      leftCup.transform.localScale = Vector3.one * 0.5f;
//      rightCup = Instantiate(cupPrefab, transform.position + rightCupOffset, Quaternion.identity, transform);
//      rightCup.transform.localScale = Vector3.one * 0.5f;
//     }
//     
//     private void CheckTablePlacement()
//     {
//         if (Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, placeDistance, tableLayer))
//         {
//             // Close to table, auto-place
//             PlaceTrayOnTable(hit.point);
//         }
//     }
//     
//     private void PlaceTrayOnTable(Vector3 tablePoint)
//     {
//         rb.isKinematic = true;
//         rb.position = tablePoint + Vector3.up * 0.1f; // Slight offset above table
//         rb.rotation = Quaternion.identity;
//         
//         // Detach both hands
//         if (leftHand != null)
//         {
//             leftHand.StopInteractingWithObject(leftAttachPoint.GetComponent<InteractableAttachPoint>());
//         }
//         if (rightHand != null)
//         {
//             rightHand.StopInteractingWithObject(rightAttachPoint.GetComponent<InteractableAttachPoint>());
//         }
//         
//         isActive = false;
//         leftHand = null;
//         rightHand = null;
//     }
//     
// }
