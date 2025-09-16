using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string message;
    public Outline outline;

    public UnityEvent onInteraction;
    public UnityEvent onReturn;
    // Start is called before the first frame update
    void Start()
    {
        DisableOutline();
    }

    public virtual void Interact(GameObject player)
    {
        onInteraction.Invoke();

        // most interactables will not be stuck on interaction
        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        playerInteract.NullInteracting();
    }

    public virtual void Return(GameObject player)
    {
        onReturn.Invoke();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}
