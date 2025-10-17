using UnityEngine;
using UnityEngine.InputSystem;

public class HipConsole : Interactable
{
    private bool _canInteract = true;
    [SerializeField] Transform robotBody;
    public AudioSource audioSource;

    private OverlayUIHandler uIHandler;
    void Start()
    {
        DisableOutline();
        uIHandler = LegUIHandler.Instance;
    }
    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToLegs(robotBody);

        _canInteract = false;
        if (audioSource != null)
            audioSource.Play();
        uIHandler.ShowContainer(player);
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffLegs();

        _canInteract = true;
        uIHandler.HideContainer(player);
    }


    public override bool CanInteract()
    {
        return _canInteract;
    }
}
