using UnityEngine;
public class HandConsole : Interactable
{
    public GameObject handRigTarget;
    private bool _canInteract = true;

    void Start()
    {
        DisableOutline();
    }

    public override void Interact(GameObject player)
    {
        if (!_canInteract) return;  // check if there is already a player on the console
        player.GetComponent<Player>().TurnOff();
        handRigTarget.GetComponent<HandMovement>().TurnOn(player);
        _canInteract = false;
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        handRigTarget.GetComponent<HandMovement>().TurnOff(player);
        _canInteract= true; // current player leaves
    }

    public override bool CanInteract()
    {
        return _canInteract;
    }
}