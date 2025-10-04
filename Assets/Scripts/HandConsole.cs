using UnityEngine;

public class HandConsole : Interactable
{
    public GameObject handRigTarget;
    public HeadConsole headConsole; // for grapple arm jamming
    private bool _canInteract = true;
    public bool left;

    private GameObject _currPlayer;

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
        _currPlayer = player;
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        handRigTarget.GetComponent<HandMovement>().TurnOff(player);
        _canInteract = true; // current player leaves
        _currPlayer = null;
    }

    public override bool CanInteract()
    {
        return _canInteract;
    }

    public void DisableInteract()
    {
        hoverMessage = "[GRAPPL DISABLED] Enter Arm to repair";
        msgColour = new Color(1, 0, 0, 1);
        outlineColour = new Color(1, 0, 0, 1);
        // handRigTarget.GetComponent<HandMovement>().JamArm(true);
        headConsole.JamArm(left, true);
        _currPlayer = null;
    }

    public void EnableInteract()
    {
        _canInteract = true;
        hoverMessage = "Control Arm";
        msgColour = new Color(1, 1, 1, 1);
        outlineColour = new Color(1, 1, 1, 1);
        // handRigTarget.GetComponent<HandMovement>().JamArm(false);
        headConsole.JamArm(left, false);
    }
}
