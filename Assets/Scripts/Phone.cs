using UnityEngine;

public class Phone : InteractableObject
{

    private Vector3 ogPosition;
    private Quaternion ogRotation;
    private Transform parent;
    private bool _canPickup;

    protected override void Start()
    {
        base.Start();
        ogPosition = transform.localPosition;
        ogRotation = transform.localRotation;
        parent = transform.parent;
        _canPickup = true;
    }

    public override void InteractWithHand(Transform obj)
    {
        if (_canPickup)
        {
            // move to hand
            DisableOutline();
            transform.parent = obj;
            transform.localPosition = new Vector3(0.0f, 5.2f, -1.0f);
            transform.localRotation = Quaternion.Euler(-88f, 10f, 0f);
            _canPickup = false;
            Debug.Log("pickup success");
        }
    }

    public override void StopInteractWithHand()
    {
        // return to original position
        transform.parent = parent;
        transform.localPosition = ogPosition;
        transform.localRotation = ogRotation;
        _canPickup = true;
    }
}
