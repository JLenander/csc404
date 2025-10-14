using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Transform Destination;
    public bool locked;

    private void Start()
    {
        DisableOutline();
        LockDoor();
    }

    public override void Interact(GameObject player)
    {
        if (!locked)
        {
            CharacterController charControl = player.GetComponent<CharacterController>();
            charControl.enabled = false;
            player.transform.position = Destination.position;
            charControl.enabled = true;
        }

        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        playerInteract.NullInteracting();
    }

    public void UnlockDoor()
    {
        locked = false;
        hoverMessage = "Control Arm";
        msgColour = new Color(1, 1, 1, 1);
        outlineColour = new Color(1, 1, 1, 1);
    }
    public void LockDoor()
    {
        locked = true;
        hoverMessage = "[DOOR LOCKED] Unlock at Brain";
        msgColour = new Color(1, 0, 0, 1);
        outlineColour = new Color(1, 0, 0, 1);
    }
}
