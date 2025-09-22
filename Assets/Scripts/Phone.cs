using UnityEngine;

public class Phone : InteractableObject
{

    private bool _isPickedUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void InteractWithHand()
    {
        if (canInteract)
        {
            Debug.Log("pickup success");
        }
    }

    public override void StopInteractWithHand()
    {
        _isPickedUp = false;    
        // return to og position
    }
}
