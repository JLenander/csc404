using UnityEngine;
using UnityEngine.InputSystem;

public class HipConsole : Interactable
{
    private bool _canInteract = true;
    [SerializeField] Transform robotBody;

    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToLegs(robotBody);

        _canInteract = false;
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffLegs();

        _canInteract = true;
    }


    public override bool CanInteract()
    {
        return _canInteract;
    }
}
