using UnityEngine;

public class HandConsole : Interactable
{
    public GameObject handRigTarget;

    void Start()
    {
        DisableOutline();
    }

    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        handRigTarget.GetComponent<HandMovement>().TurnOn(player);
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        handRigTarget.GetComponent<HandMovement>().TurnOff(player);
    }
}
