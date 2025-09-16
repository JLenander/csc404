using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Transform Destination;
    [SerializeField] private float range = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    public override void Interact(GameObject player)
    {
        CharacterController charControl = player.GetComponent<CharacterController>();
        charControl.enabled = false;
        player.transform.position = Destination.position;
        charControl.enabled = true;

        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        playerInteract.NullInteracting();
    }
}
