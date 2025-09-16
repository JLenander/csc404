using UnityEngine;
using UnityEngine.InputSystem;

public class HeadConsole : Interactable
{
    [SerializeField] private Transform camPosition;
    [SerializeField] private Quaternion camAngle;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public override void Interact(GameObject player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();

        // save old rotation
        Camera playerCam = playerInput.camera;
        originalPosition = playerCam.transform.position;
        originalRotation = playerCam.transform.rotation;

        playerCam.transform.position = camPosition.position;
        playerCam.transform.rotation = camAngle;

        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToHead();
    }

    public override void Return(GameObject player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        Camera playerCam = playerInput.camera;

        playerCam.transform.position = originalPosition;
        playerCam.transform.rotation = originalRotation;

        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffHead();
    }
}
