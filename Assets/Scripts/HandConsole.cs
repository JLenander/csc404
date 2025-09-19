using UnityEngine;

public class HandConsole : Interactable
{
    public GameObject handRigTarget;
    private bool _hasPlayer = false;
    private bool _interactionSuccess = false;

    void Start()
    {
        DisableOutline();
    }

    public override void Interact(GameObject player)
    {
        if (_hasPlayer) {
            _interactionSuccess = false;
            return;
        }
        player.GetComponent<Player>().TurnOff();
        handRigTarget.GetComponent<HandMovement>().TurnOn(player);
        _hasPlayer = true;
        _interactionSuccess = true;
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        handRigTarget.GetComponent<HandMovement>().TurnOff(player);
        _hasPlayer= false;
        _interactionSuccess = false;
    }

    public override bool InteractionSuccess()
    {
        return _interactionSuccess;
    }
}
