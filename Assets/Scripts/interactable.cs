using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    // The message that shows on the screen when a player hovers over this interactable
    [FormerlySerializedAs("message")] public string hoverMessage;
    public Color msgColour = new Color(1f, 1f, 1f, 1f);
    public Outline outline;

    public Color outlineColour = new Color(1f, 1f, 1f, 1f);

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
        outline.OutlineColor = outlineColour;
        outline.enabled = true;
    }

    public virtual bool InteractionSuccess()
    {
        return true;
    }
    public virtual bool CanInteract()
    {
        return true;
    }
}
