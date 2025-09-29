using UnityEngine;
using UnityEngine.InputSystem;

public class HeadConsole : Interactable
{
    [SerializeField] private Transform exteriorHead; //reference for robot head
    [SerializeField] private Quaternion camAngle; // current robot head angle
    private Quaternion originalPlayerRotation; // player rotation
    private Vector3 originalCamPosition; // player camera position
    private Quaternion originalCamLocalRotation; // player camera local rotation
    private Transform camParent; // camera's parent (player object)

    private bool _canInteract = true;

    public override void Interact(GameObject player)
    {
        if (!_canInteract) return;
        PlayerInput playerInput = player.GetComponent<PlayerInput>();

        // get camera from player input and save old rotation and position
        Camera playerCam = playerInput.camera;
        originalCamPosition = playerCam.transform.position;
        originalPlayerRotation = player.transform.localRotation;
        originalCamLocalRotation = playerCam.transform.localRotation;
        camParent = playerCam.transform.parent; //save player camera's parent
        playerCam.transform.parent = exteriorHead;// make the exterior head the new parent

        // teleport camera to exterior head and align angle
        playerCam.transform.position = exteriorHead.position;
        playerCam.transform.rotation = camAngle;

        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToHead();
        _canInteract = false;
    }

    public override void Return(GameObject player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        Camera playerCam = playerInput.camera;

        // reinput player's data
        playerCam.transform.parent = camParent;
        playerCam.transform.position = originalCamPosition;
        player.transform.rotation = originalPlayerRotation;
        playerCam.transform.localRotation = originalCamLocalRotation;

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
