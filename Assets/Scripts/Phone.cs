using UnityEngine;

public class Phone : InteractableObject
{
    public Collider triggerCollider;
    private Vector3 ogPosition;
    private Quaternion ogRotation;
    private Transform parent;
    private Rigidbody rg;

    public override void Start()
    {
        base.Start();
        ogPosition = transform.localPosition;
        ogRotation = transform.localRotation;
        parent = transform.parent;

        rg = GetComponent<Rigidbody>();
    }

    public override void InteractWithHand(Transform obj, HandMovement target)
    {
        if (canInteract && canPickup)
        {
            // move to hand
            DisableOutline();
            transform.parent = obj;
            transform.localPosition = new Vector3(0.0f, 5.2f, -1.0f);
            transform.localRotation = Quaternion.Euler(-88f, 10f, 0f);
            canPickup = false;

            rg.isKinematic = true;
            triggerCollider.enabled = false;
            Debug.Log("pickup success");

            target.SetTargetCurrentObject(this);
            target.oppositeHandAnimator.SetTrigger("Point"); // sets the opposite hand to point
            target.handAnimator.SetTrigger("Hold"); // sets current hand to hold anim
        }
    }

    public override void StopInteractWithHand(HandMovement target)
    {
        // return to original position
        transform.parent = parent;
        // transform.localPosition = ogPosition;
        // transform.localRotation = ogRotation;
        canPickup = true;

        rg.isKinematic = false;
        triggerCollider.enabled = true;
        target.oppositeHandAnimator.SetTrigger("Neutral"); // sets the opposite hand back to neutral
        target.handAnimator.SetTrigger("Neutral"); // sets the current hand back to neutral
    }
}
