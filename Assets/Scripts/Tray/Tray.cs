using System.Collections;
using UnityEngine;

public class Tray : InteractableObject
{
    [Header("Setup")]
    private Rigidbody rb;
    
    [Header("Interaction Colliders")]
    public Collider leftGrappleCollider;
    public Collider rightGrappleCollider;

    [Header("Attach Points")]
    public Transform leftAttachSpawn;
    public Transform rightAttachSpawn;

    [Header("Placement")]
    public Transform table;
    public float placeDistance = 0.5f;

    [Header("Tilt Settings")]
    public float tiltSensitivity = 20f;
    public float followSpeed = 10f;

    private Transform firstHand = null;
    private Transform secondHand = null;
    private bool trayActive = false;
    private bool placed = false;
    
    public override void Start()
    {
        base.Start();
        DisableOutline();
        rb = GetComponent<Rigidbody>();
        Debug.Log("Tray canInteract = " + canInteract);
        Debug.Log("Tray canPickup = " + canPickup);
    }

    public override void InteractWithHand(Transform hand, HandMovement target)
    {
        Debug.Log("Interact " + hand.name);
        if (!canInteract || placed) return;
        
        Debug.Log("Can Interact With" + hand.name);

        // Assign first free attach point
        if (firstHand == null)
        {
            firstHand = hand;
        }
        else if (secondHand == null && hand != firstHand)
        {
            secondHand = hand;
        }
        else
        {
            return; // already two hands attached
        }

        // Snap hand to attach point, NOT other way around, need tray stay on table until picked up
        hand.position = GetClosestAttachPoint(hand).position;
        hand.rotation = GetClosestAttachPoint(hand).rotation;
        hand.SetParent(transform);

        target.SetTargetCurrentObject(this);
        target.handAnimator.SetTrigger("Pot");

        // Activate tray when two hands attached
        if (firstHand != null && secondHand != null && !trayActive)
        {
            rb.isKinematic = false;
            trayActive = true;
            Debug.Log("Tray active with two hands");
        }
    }

    public override void StopInteractWithHand(HandMovement target)
    {
        Transform hand = target.transform;

        if (hand == firstHand) firstHand = null;
        if (hand == secondHand) secondHand = null;

        hand.SetParent(null);
        target.handAnimator.SetTrigger("Neutral");

        // Deactivate tray if no hands
        if (firstHand == null && secondHand == null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            trayActive = false;

            // Auto-place if near table
            if (Vector3.Distance(transform.position, table.position) < placeDistance)
            {
                PlaceOnTable();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!trayActive || firstHand == null || secondHand == null) return;

        // Move tray to center of hands
        Vector3 targetPos = (firstHand.position + secondHand.position) / 2f;
        rb.linearVelocity = (targetPos - rb.position) * followSpeed;

        // Tilt tray based on hand height difference
        float tiltX = (secondHand.position.y - firstHand.position.y) * tiltSensitivity;
        float tiltZ = (secondHand.position.z - firstHand.position.z) * tiltSensitivity;

        Quaternion targetRot = Quaternion.Euler(tiltX, 0f, tiltZ);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 5f));
    }

    private Transform GetClosestAttachPoint(Transform hand)
    {
        // Returns whichever attach point is free and closest
        if ((firstHand != null && firstHand != hand) && (secondHand != null && secondHand != hand))
            return null; // both taken

        // One attach point free
        if (firstHand == null || firstHand == hand) return leftAttachSpawn;
        if (secondHand == null || secondHand == hand) return rightAttachSpawn;
        
        float distLeft = Vector3.Distance(hand.position, leftAttachSpawn.position);
        float distRight = Vector3.Distance(hand.position, rightAttachSpawn.position);
        
        return distLeft <= distRight ? leftAttachSpawn : rightAttachSpawn;
    }

    private void PlaceOnTable()
    {
        placed = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = table.position + Vector3.up * 0.05f;
        transform.rotation = Quaternion.identity;

        Debug.Log("Tray placed on table");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tray trigger entered by: " + other.name);
    }
}
