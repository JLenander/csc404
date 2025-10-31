using UnityEngine;
using UnityEngine.InputSystem;

public class HipConsole : Interactable
{
    private bool _canInteract = true;
    [SerializeField] Transform robotBody;
    public AudioSource audioSource;

    private OverlayUIHandler uIHandler;

    [SerializeField] Transform playerChair;
    private TriggerSeat triggerSeat;
    void Start()
    {
        DisableOutline();
        uIHandler = LegUIHandler.Instance;
        if (playerChair) triggerSeat = playerChair.GetComponent<TriggerSeat>();

    }
    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToLegs(robotBody);

        _canInteract = false;
        if (audioSource != null)
            audioSource.Play();
        uIHandler.ShowContainer(player);
        triggerSeat.StandRobot();
        if (Level1TaskManager.Instance.GetTaskData("Leave") == null && playerChair)
        {
            triggerSeat.sceneExitDoor.enabled = false;
            Collider collider = triggerSeat.GetComponent<Collider>();
            collider.enabled = true;
        }
    }

    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffLegs();

        _canInteract = true;
        uIHandler.HideContainer(player);
        if (playerChair && triggerSeat.PlayerInsideSeat())
        {
            triggerSeat.SeatRobot();
        }
    }


    public override bool CanInteract()
    {
        return _canInteract;
    }
}
