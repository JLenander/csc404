using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Transform Destination;
    public bool locked;

    private void Start()
    {
        DisableOutline();
        locked = true;
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
    }
    public void LockDoor()
    {
        locked = true;
    }
}
