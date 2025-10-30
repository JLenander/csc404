// using UnityEngine;
//
// public class TrayController : MonoBehaviour
// {
//     [Header("Hands")]
//     public Transform leftHand;
//     public Transform rightHand;
//
//     [Header("Attach Points")]
//     public Transform leftAttachPoint;
//     public Transform rightAttachPoint;
//
//     [Header("Table Placement")]
//     public Transform table;
//     public float placeDistance = 0.5f;
//
//     [Header("Cup Settings")]
//     public GameObject cupPrefab;
//     private Vector3 leftCupOffset = new Vector3(2.64f, 2f, 2.68f);
//     private Vector3 rightCupOffset = new Vector3(-0.58f, 2f, 0.4f);
//     
//     [Header("Tray Respawn")]
//     public GameObject trayPrefab;   // assign same tray prefab
//     public Transform counterSpawnPoint; // position to respawn tray
//
//     [Header("Tray Settings")]
//     public float magnetDistance = 0.2f;
//
//     private Rigidbody rb;
//     private bool leftHolding = false;
//     private bool rightHolding = false;
//     private bool trayActive = false;
//     private GameObject leftCup;
//     private GameObject rightCup;
//     public float dropThreshold = 0.1f; // y position where a cup is considered dropped
//     private bool cupsDropped = false;
//
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.isKinematic = true;
//
//         SpawnCups();
//         // ensure cups start kinematic (on counter)
//         SetCupsKinematic(true);
//     }
//     
//     public void AttachHand(Transform wristBone, bool isLeft)
//     {
//         rb ??= GetComponent<Rigidbody>();
//         rb.isKinematic = false;
//
//         Transform target = isLeft ? leftAttachPoint : rightAttachPoint;
//         wristBone.position = target.position;
//         wristBone.rotation = target.rotation;
//
//         if (isLeft) leftHolding = true;
//         else rightHolding = true;
//         
//         // When a hand attaches, if both hands attached we activate physics
//         if (leftHolding && rightHolding && !trayActive)
//         {
//             rb.isKinematic = false;
//             trayActive = true;
//             SetCupsKinematic(false); // allow cups to slide/fall
//         }
//     }
//
//     public void DetachHand(bool isLeft)
//     {
//         if (isLeft) leftHolding = false;
//         else rightHolding = false;
//
//         if (!leftHolding && !rightHolding)
//         {
//             // both hands released -> freeze tray until placed again
//             rb.isKinematic = true;
//             trayActive = false;
//
//             if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
//             {
//                 if (hit.collider.CompareTag("Table"))
//                 {
//                     rb.MovePosition(hit.point + Vector3.up * 0.02f);
//                     rb.MoveRotation(Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0));
//                     PlaceOnTable();
//                 }
//             }
//         }
//
//     }
//     
//     private void Update()
//     {
//         // Per-frame magnet handling for free snapping (works even if external AttachHand not used)
//         HandleHandMagnet(leftHand, leftAttachPoint, ref leftHolding);
//         HandleHandMagnet(rightHand, rightAttachPoint, ref rightHolding);
//
//         // Activate tray physics when both hands hold it
//         if (leftHolding && rightHolding && !trayActive)
//         {
//             rb.isKinematic = false;
//             trayActive = true;
//             SetCupsKinematic(false); // cups become dynamic
//         }
//
//         // Follow hands and tilt while tray is active
//         if (trayActive)
//         {
//             Vector3 targetPos = (leftAttachPoint.position + rightAttachPoint.position) / 2f;
//             Quaternion targetRot = Quaternion.LookRotation(rightAttachPoint.position - leftAttachPoint.position, Vector3.up);
//
//             rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * followSpeed));
//             rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * followSpeed));
//         }
//
//         // Auto-place on table when both hands released and close to table
//         if (!leftHolding && !rightHolding && Vector3.Distance(transform.position, table.position) < placeDistance)
//         {
//             PlaceOnTable();
//         }
//
//         // Cup drop checks
//         CheckCupDrop(ref leftCup);
//         CheckCupDrop(ref rightCup);
//     }
//
//     void HandleHandMagnet(Transform hand, Transform attachPoint, ref bool isHolding)
//     {
//         if (hand == null || attachPoint == null) return;
//
//         if (!isHolding)
//         {
//             if (Vector3.Distance(hand.position, attachPoint.position) < magnetDistance)
//             {
//                 isHolding = true;
//                 // Snap instantly to attach point
//                 hand.position = attachPoint.position;
//                 hand.rotation = attachPoint.rotation;
//
//                 // If both hands now hold -> activate tray
//                 if (leftHolding && rightHolding && !trayActive)
//                 {
//                     rb.isKinematic = false;
//                     trayActive = true;
//                     SetCupsKinematic(false);
//                 }
//             }
//         }
//         else
//         {
//             // While holding, keep the hand aligned to the attach point smoothly
//             hand.position = Vector3.Lerp(hand.position, attachPoint.position, Time.deltaTime * magnetSnapSpeed);
//             hand.rotation = Quaternion.Slerp(hand.rotation, attachPoint.rotation, Time.deltaTime * magnetSnapSpeed);
//         }
//     }
//     
//     void SpawnCups()
//     {
//         leftCup = Instantiate(cupPrefab, transform.position + leftCupOffset, Quaternion.identity, transform);
//         leftCup.transform.localScale = Vector3.one * 0.5f;
//         rightCup = Instantiate(cupPrefab, transform.position + rightCupOffset, Quaternion.identity, transform);
//         rightCup.transform.localScale = Vector3.one * 0.5f;
//     }
//     
//     void CheckCupDrop(GameObject cup)
//     {
//         if (cupsDropped) return;
//
//         if (cup == null)
//         {
//             // already destroyed
//             EvaluateCupsDestroyed();
//             return;
//         }
//
//         if (cup.transform.position.y < dropThreshold)
//         {
//             Destroy(cup);
//             cup = null;
//             EvaluateCupsDestroyed();
//         }
//     }
//     
//     private void EvaluateCupsDestroyed()
//     {
//         if ((leftCup == null) && (rightCup == null))
//         {
//             cupsDropped = true;
//             DestroyTrayAndRespawn();
//         }
//     }
//
//     private void DestroyTrayAndRespawn()
//     {
//         if (trayPrefab != null && counterSpawnPoint != null)
//         {
//             Instantiate(trayPrefab, counterSpawnPoint.position, Quaternion.identity);
//         }
//         Destroy(gameObject);
//     }
//     
//     private void SetCupsKinematic(bool kinematic)
//     {
//         if (leftCup)
//         {
//             Rigidbody lr = leftCup.GetComponent<Rigidbody>();
//             if (lr) lr.isKinematic = kinematic;
//         }
//
//         if (rightCup)
//         {
//             Rigidbody rr = rightCup.GetComponent<Rigidbody>();
//             if (rr) rr.isKinematic = kinematic;
//         }
//     }
//     
//     void PlaceOnTable()
//     {
//         transform.position = table.position + Vector3.up * 0.05f;
//         transform.rotation = Quaternion.identity;
//     
//         rb.isKinematic = true;
//         trayActive = false;
//
//         // Make cups kinematic so they stay on tray/table
//         SetCupsKinematic(true);
//     }
// }










//----------------------------------------------------------------------------------------------------------------\\

// using UnityEngine;
//
// public class TrayController : MonoBehaviour
// {
//     [Header("Hands")]
//     public Transform leftHand;
//     public Transform rightHand;
//
//     [Header("Attach Points")]
//     public Transform leftAttachPoint;
//     public Transform rightAttachPoint;
//
//     [Header("Table Placement")]
//     public Transform table;
//     public float placeDistance = 0.5f;
//
//     [Header("Cup Settings")]
//     public GameObject cupPrefab;
//     public Vector3 leftCupOffset = new Vector3(2.64f, 2f, 2.68f);
//     public Vector3 rightCupOffset = new Vector3(-0.58f, 2f, 0.4f);
//     
//     [Header("Tray Respawn")]
//     public GameObject trayPrefab;   // assign same tray prefab
//     public Transform counterSpawnPoint; // position to respawn tray
//
//     [Header("Tray Settings")]
//     public float magnetDistance = 20f;
//     public float dropThreshold = 0.2f; // y position where a cup is considered dropped
//
//     private Rigidbody rb;
//     private bool leftHolding = false;
//     private bool rightHolding = false;
//     private bool trayActive = false;
//     private GameObject leftCup;
//     private GameObject rightCup;
//
//     public Outline outline;
//
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.isKinematic = true;
//
//         SpawnCups();
//     }
//
//     void Update()
//     {
//         HandleHandMagnet(leftHand, leftAttachPoint, ref leftHolding);
//         HandleHandMagnet(rightHand, rightAttachPoint, ref rightHolding);
//
//         // When both hands attached, activate tray physics
//         if (leftHolding && rightHolding && !trayActive)
//         {
//             rb.isKinematic = false;
//             trayActive = true;
//         }
//
//         // When holding, tray follows hands
//         if (trayActive && leftHolding && rightHolding)
//         {
//             // Vector3 targetCenter = (leftAttachPoint.position + rightAttachPoint.position) / 2f;
//             // Vector3 move = targetCenter - rb.position;
//             Vector3 targetPos = (leftAttachPoint.position + rightAttachPoint.position) / 2f;
//             rb.MovePosition(targetPos);
//
//             // Tilt tray based on relative hand height
//             Vector3 handDir = rightAttachPoint.position - leftAttachPoint.position;
//             Quaternion targetRot = Quaternion.Euler(-handDir.y, 0f, handDir.z);
//             rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * 5f));
//         }
//
//         // Auto-place on table
//         if (!leftHolding && !rightHolding && Vector3.Distance(transform.position, table.position) < placeDistance)
//         {
//             PlaceOnTable();
//         }
//
//         // Cup drop check
//         CheckCupDrop(leftCup);
//         CheckCupDrop(rightCup);
//     }
//
//     void HandleHandMagnet(Transform hand, Transform attachPoint, ref bool isHolding)
//     {
//         if (!isHolding && Vector3.Distance(hand.position, attachPoint.position) < magnetDistance)
//         {
//             isHolding = true;
//             hand.position = attachPoint.position; //not?
//         }
//     }
//
//     void SpawnCups()
//     {
//         leftCup = Instantiate(cupPrefab, transform.position + leftCupOffset, Quaternion.identity, transform);
//         leftCup.transform.localScale = Vector3.one * 0.5f;
//         rightCup = Instantiate(cupPrefab, transform.position + rightCupOffset, Quaternion.identity, transform);
//         rightCup.transform.localScale = Vector3.one * 0.5f;
//     }
//
//     void CheckCupDrop(GameObject cup)
//     {
//         if (cup == null) return;
//         if (cup.transform.position.y < dropThreshold)
//         {
//             Destroy(cup);
//
//             // If both cups gone, destroy tray and respawn new tray (for prototype just respawn cups)
//             if ((leftCup == null || leftCup == cup) && (rightCup == null || rightCup == cup))
//             {
//                 Destroy(gameObject); // tray gone
//                 
//                 // Spawn new tray
//                 if (trayPrefab && counterSpawnPoint)
//                 {
//                     Instantiate(trayPrefab, counterSpawnPoint.position, Quaternion.identity);
//                 }
//             }
//         }
//     }
//
//     void PlaceOnTable()
//     {
//         transform.position = table.position + Vector3.up * 0.05f;
//         transform.rotation = Quaternion.identity;
//
//         rb.isKinematic = true;
//
//         foreach (Transform cup in transform)
//         {
//             Rigidbody cupRb = cup.GetComponent<Rigidbody>();
//             if (cupRb)
//                 cupRb.isKinematic = true;
//         }
//     }
//     
//     public void DisableOutline()
//     {
//         outline.enabled = false;
//     }
//     
//     public void EnableOutline()
//     {
//         outline.enabled = true;
//     }
// }

