using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InteractableObject : MonoBehaviour
{
    public Outline outline;
    public bool canInteract = true;
    private HandMovement hand;

    protected virtual void Start()
    {
        DisableOutline();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other != null)
        {
            EnableOutline();
            canInteract = true;
            hand = other.gameObject.GetComponent<Wrist>().target.GetComponent<HandMovement>();
            hand.SetCurrentInteractableObject(gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DisableOutline();
        canInteract = false;
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

    public virtual void InteractWithHand(Transform obj)
    {
    }

    public virtual void StopInteractWithHand()
    {
    }
}
