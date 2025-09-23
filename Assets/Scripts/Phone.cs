using UnityEngine;

public class Phone : InteractableObject
{

    private Vector3 ogPosition;
    private Quaternion ogRotation;
    private Transform parent;

    protected override void Start()
    {
        base.Start();
        ogPosition = transform.localPosition;
        ogRotation = transform.localRotation;
        parent = transform.parent;
    }

    public override void InteractWithHand(Transform obj)
    {
        if (canInteract && canPickup)
        {
            // move to hand
            DisableOutline();
            transform.parent = obj;
            transform.localPosition = new Vector3(0.0f, 5.2f, -1.0f);
            transform.localRotation = Quaternion.Euler(-88f, 10f, 0f);
            canPickup = false;
            Debug.Log("pickup success");
        }
    }

    public override void StopInteractWithHand()
    {
        // return to original position
        transform.parent = parent;
        transform.localPosition = ogPosition;
        transform.localRotation = ogRotation;
        canPickup = true;
    }
}
