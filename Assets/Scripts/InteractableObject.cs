using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public abstract class InteractableObject : MonoBehaviour
{
    public Outline outline;
    public bool canInteract = true;
    public bool canPickup = true;
    private HandMovement handMovement;
    private Hand hand;

    public virtual void Start()
    {
        DisableOutline();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Hand"))
        {
            EnableOutline();
            canInteract = true;
            hand = other.GetComponent<Hand>();
            if (hand != null)
            {
                handMovement = hand.GetHandMovement();
                handMovement.SetCurrentInteractableObject(gameObject, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DisableOutline();
        canInteract = false;
        if (handMovement != null)
            handMovement.SetCurrentInteractableObject(null, false);
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public virtual void InteractWithHand(Transform wrist, HandMovement target)
    {
    }

    public virtual void StopInteractWithHand(HandMovement target)
    {
    }
}
