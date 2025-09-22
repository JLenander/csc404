using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InteractableObject : MonoBehaviour
{
    public Outline outline;
    public bool canInteract = true;
    private HandMovement hand;
    private InputAction _interactAction;
    private GameObject _currPlayer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableOutline();
        //handMovement = FindFirstObjectByType<HandMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other != null)
        {
            EnableOutline();
            canInteract = true;
            hand = other.gameObject.transform.parent.parent.parent.Find("Hand Rig/Rig_target").GetComponent<HandMovement>();
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

    public virtual void InteractWithHand()
    {
    }

    public virtual void StopInteractWithHand()
    {
    }
}
