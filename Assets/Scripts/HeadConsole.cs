using UnityEngine;
using UnityEngine.InputSystem;

public class HeadConsole : Interactable
{
    private ISplitscreenUIHandler _splitscreenUIHandler;

    [SerializeField] private Camera exteriorCamera; //reference for robot head camera
    [SerializeField] private Quaternion camAngle; // current robot head angle

    private bool _canInteract = true;

    void Start()
    {
        _splitscreenUIHandler = FindAnyObjectByType<SplitscreenUIHandler>();
    }
    
    public override void Interact(GameObject player)
    {
        if (!_canInteract) return;
        _splitscreenUIHandler.ShowOutsideCamera();

        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToHead(exteriorCamera);
        _canInteract = false;
    }

    public override void Return(GameObject player)
    {
        _splitscreenUIHandler.HideOutsideCamera();

        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffHead();

        _canInteract = true;
    }
    public override bool CanInteract()
    {
        return _canInteract;
    }

    public void disableInteract()
    {
        _canInteract = false;
        hoverMessage = "[DISABLED] Please Blink";
        msgColour = new Color(1, 0, 0, 1);
        outlineColour = new Color(1, 0, 0, 1);
    }

    public void enableInteract()
    {
        _canInteract = true;
        hoverMessage = "Control Head";
        msgColour = new Color(1, 1, 1, 1);
        outlineColour = new Color(1, 1, 1, 1);
    }
}
