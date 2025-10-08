using UnityEngine;
using UnityEngine.InputSystem;

public class HeadConsole : Interactable
{
    private ISplitscreenUIHandler _splitscreenUIHandler;

    [SerializeField] private Camera exteriorCamera; //reference for robot head camera
    [SerializeField] private Quaternion camAngle; // current robot head angle

    private bool _canInteract = true;

    [SerializeField] private GameObject leftGrappleArmSpline;
    [SerializeField] private GameObject rightGrappleArmSpline;

    private bool _leftJammed, _rightJammed;
    private bool _leftShot, _rightShot;
    private InputAction _leftTriggerAction, _rightTriggerAction;
    private GameObject _currPlayer;

    public AudioSource hookSource;

    void Start()
    {
        DisableOutline();
        _splitscreenUIHandler = FindAnyObjectByType<SplitscreenUIHandler>();
        // for grapple arm
        _leftJammed = _rightJammed = false;
        _leftShot = _rightShot = false;
    }

    // for grapple arm, check trigger input to shoot or retract
    void Update()
    {
        if (_currPlayer != null)
        {
            // Left arm
            if (_leftTriggerAction != null && _leftTriggerAction.ReadValue<float>() > 0.1f && !_leftJammed)
            {
                if (!_leftShot)
                {
                    _leftShot = true;
                    EmergencyEvent.Instance.IncrementCount(true); // or pass correct value

                    if (hookSource != null)
                        hookSource.Play();
                }
                leftGrappleArmSpline.GetComponent<SplineController>().SetExtending();
            }
            else
            {
                if (_leftShot)
                {
                    _leftShot = false;
                }
                leftGrappleArmSpline.GetComponent<SplineController>().SetRetracting();
            }

            // Right arm
            if (_rightTriggerAction != null && _rightTriggerAction.ReadValue<float>() > 0.1f && !_rightJammed)
            {
                if (!_rightShot)
                {
                    _rightShot = true;
                    EmergencyEvent.Instance.IncrementCount(false);

                    if (hookSource != null)
                        hookSource.Play();
                }
                rightGrappleArmSpline.GetComponent<SplineController>().SetExtending();
            }
            else
            {
                if (_rightShot)
                    _rightShot = false;
                rightGrappleArmSpline.GetComponent<SplineController>().SetRetracting();
            }
        }
    }

    public override void Interact(GameObject player)
    {
        if (!_canInteract) return;
        _splitscreenUIHandler.ShowOutsideCamera();

        player.GetComponent<Player>().TurnOff();
        player.GetComponent<Player>().switchToHead(exteriorCamera);
        _canInteract = false;

        // for grapple arm
        _currPlayer = player;
        var input = _currPlayer.GetComponent<PlayerInput>();
        _leftTriggerAction = input.actions.FindAction("LeftTrigger");
        _rightTriggerAction = input.actions.FindAction("RightTrigger");
    }

    public override void Return(GameObject player)
    {
        _splitscreenUIHandler.HideOutsideCamera();

        player.GetComponent<Player>().TurnOn();
        player.GetComponent<Player>().switchOffHead();

        _canInteract = true;

        // for grapple arm
        _currPlayer = null;
        _leftTriggerAction = null;
        _rightTriggerAction = null;
    }
    public override bool CanInteract()
    {
        return _canInteract;
    }

    public void DisableInteract()
    {
        _canInteract = false;
        hoverMessage = "[DISABLED] Please Blink";
        msgColour = new Color(1, 0, 0, 1);
        outlineColour = new Color(1, 0, 0, 1);
    }

    public void EnableInteract()
    {
        _canInteract = true;
        hoverMessage = "Control Head";
        msgColour = new Color(1, 1, 1, 1);
        outlineColour = new Color(1, 1, 1, 1);
    }

    // for grapple arm - to be called by HandConsole when arm is broken or fixed
    public void JamArm(bool left, bool state)
    {
        if (left)
            _leftJammed = state;
        else
            _rightJammed = state;
    }
}
