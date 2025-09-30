using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InteractableObject : MonoBehaviour
{
    public Outline outline;
    public bool canInteract = true;
    public bool canPickup = true;
    private HandMovement hand;

    protected virtual void Start()
    {
        DisableOutline();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Hand"))
        {
            EnableOutline();
            canInteract = true;
            hand = other.GetComponent<HandMovement>();
            if (hand != null)
            {
                hand.SetCurrentInteractableObject(gameObject, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DisableOutline();
        canInteract = false;
        if (hand != null)
            hand.SetCurrentInteractableObject(null, false);
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
