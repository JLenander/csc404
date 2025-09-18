using UnityEngine;
using UnityEngine.InputSystem;

public class HeadConsole : Interactable
{
    [SerializeField] private Transform exteriorHead; //reference for robot head
    [SerializeField] private Quaternion camAngle; // current robot head angle
    private Vector3 originalPosition; // player camera position
    private Quaternion originalRotation; // player camera rotation
    private Transform parent; // camera's parent (player object)

    public override void Interact(GameObject player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();

        // get camera from player input and save old rotation and position
        Camera playerCam = playerInput.camera;
        originalPosition = playerCam.transform.position;
        originalRotation = playerCam.transform.rotation;
        parent = playerCam.transform.parent; //save player camera's parent
        playerCam.transform.parent = exteriorHead;// make the exterior head the new parent

        // teleport camera to exterior head and align angle
        playerCam.transform.position = exteriorHead.position;
        playerCam.transform.rotation = camAngle;

        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToHead();
    }

    public override void Return(GameObject player)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        Camera playerCam = playerInput.camera;

        // reinput player's data
        playerCam.transform.position = originalPosition;
        playerCam.transform.rotation = originalRotation;
        playerCam.transform.parent = parent;

        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffHead();
    }
}
