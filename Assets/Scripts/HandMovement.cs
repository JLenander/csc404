using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovement : MonoBehaviour
{
    public float speed = 5f;

    private InputAction _moveAction;
    private InputAction _leftTriggerAction;
    private InputAction _rightTriggerAction;
    private InputAction _lookAction;
    private InputAction _interactAction;

    public Vector3 movement = Vector3.zero;
    private Vector3 _ogPosition;
    private bool _disable;

    private bool _isMoving;
    public AudioSource moveSource;
    public AudioSource stopSource;

    private GameObject _currPlayer;

    public float lookSensitivity = 0.4f;

    [SerializeField] public Transform _wrist;
    [SerializeField] public Transform _wristAim;

    public Animator oppositeHandAnimator; // animator of opposite hand
    public Animator handAnimator;
    public float wristRotateX;
    public float wristRotateY;
    private GameObject _toInteractObj;  // check which object is it colliding with
    private InteractableObject _currObj;    // currently interacting with hand
    private bool _canInteract;  // can interact status

    [SerializeField] private GameObject grappleArmSpline;
    public bool left;

    private void Start()
    {
        _ogPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_disable)
        {
            // hand rigid body movement
            Vector2 stickMove = _moveAction.ReadValue<Vector2>();
            Vector3 stickMovement = new Vector3(stickMove.x, stickMove.y, 0);

            float leftTrigger = _leftTriggerAction.ReadValue<float>();
            float rightTrigger = _rightTriggerAction.ReadValue<float>();
            Vector3 triggerMovement = new Vector3(0, 0, leftTrigger - rightTrigger);

            movement += (stickMovement + triggerMovement) * Time.deltaTime;
            // movement done in FixedUpdate

            // rotation movement (done in LateUpdate)
            Vector2 lookMove = _lookAction.ReadValue<Vector2>();
            wristRotateX += lookMove.x * lookSensitivity * -1.0f;
            wristRotateY += lookMove.y * lookSensitivity;
            wristRotateY = Mathf.Clamp(wristRotateY, -90f, 90f);

            // changed from movement.magnitude to this addition because movement is now += instead of =
            bool movingNow = ((stickMovement + triggerMovement).magnitude > 0.5f) || (lookMove.magnitude > 0.3f);

            // Movement started
            if (movingNow && !_isMoving)
            {
                _isMoving = true;

                // != expensive but confirmed the right approach
                if (moveSource != null && !moveSource.isPlaying)
                    moveSource.Play();
            }

            // Movement stopped
            if (!movingNow && _isMoving)
            {
                _isMoving = false;

                if (moveSource != null && moveSource.isPlaying)
                    moveSource.Stop();

                if (stopSource != null)
                    stopSource.Play();
            }

            // check if hand is empty and is there an object to interact with
            if (_interactAction.WasPressedThisFrame() && _toInteractObj != null && _canInteract && _currObj == null)
            {
                if (_toInteractObj.TryGetComponent(out InteractableObject interactable))
                {
                    if (interactable.canPickup)
                    {
                        // log grab success
                        ScoreKeeper.Instance.IncreaseArmCount(left);
                        InteractWithObject(interactable);
                        _currObj = interactable;
                        _canInteract = false;
                    }
                }
            }

            // check if hand is not empty
            else if (_interactAction.WasPressedThisFrame() && _currObj != null)
            {
                Debug.Log("interaction " + _toInteractObj + _canInteract);
                StopInteractingWithObject(_currObj);
            }
        }
        else
        {
            grappleArmSpline.GetComponent<SplineController>().SetRetracting();
            // keep arm retracting without player input
        }

    }

    // Use FixedUpdate for physics-based movement
    private void FixedUpdate()
    {
        if (left)
        {
            transform.localPosition = movement * speed + _ogPosition;
        }
        else
        {
            Vector3 tmpMvt = movement;
            tmpMvt.x *= -1.0f;
            transform.localPosition = tmpMvt * speed + _ogPosition;
        }
    }

    private void LateUpdate()
    {
        if (left)
        {
            // left arm rotation
            _wrist.localRotation = Quaternion.Euler(wristRotateY, wristRotateX, 0);
            _wristAim.localRotation = Quaternion.Euler(wristRotateY, wristRotateX * -1.0f, 0);
        }
        else
        {
            // right arm rotation
            _wrist.localRotation = Quaternion.Euler(wristRotateY, wristRotateX * -1.0f, 0);
            _wristAim.localRotation = Quaternion.Euler(wristRotateY, wristRotateX, 0);
        }
    }


    // using TurnOn to initialize when player starts using the hand, not in Start() when object instantiate
    public void TurnOn(GameObject playerUsing)
    {
        _currPlayer = playerUsing;
        var input = _currPlayer.GetComponent<PlayerInput>();
        _moveAction = input.actions.FindAction("Move");
        _leftTriggerAction = input.actions.FindAction("LeftTrigger");
        _rightTriggerAction = input.actions.FindAction("RightTrigger"); _lookAction = input.actions.FindAction("Look");
        _interactAction = input.actions.FindAction("ItemInteract");
        _disable = true;
    }

    public void TurnOff(GameObject playerUsing)
    {
        _disable = false;
    }

    public void SetCurrentInteractableObject(GameObject handUsing, bool canInteract)
    {
        _toInteractObj = handUsing;
        _canInteract = canInteract;
    }

    private void InteractWithObject(InteractableObject interactableObject)
    {
        Debug.Log("Interacting with " + interactableObject);
        interactableObject.InteractWithHand(_wrist, this);
    }

    public void StopInteractingWithObject(InteractableObject interactableObject)
    {
        Debug.Log("Stopping interaction with " + interactableObject);
        interactableObject.StopInteractWithHand(this);
        _currObj = null;
    }

}

