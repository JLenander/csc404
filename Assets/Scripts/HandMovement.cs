using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovement : MonoBehaviour
{
    public float speed = 5f;

    private InputAction _moveAction;
    private InputAction _leftTriggerAction;
    private InputAction _rightTriggerAction;
    private InputAction _leftBumperAction;
    private InputAction _rightBumperAction;
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

    [SerializeField] private float wristRotationSpeed = 1.0f;

    [SerializeField] public Transform _wrist;
    [SerializeField] public Transform _wristAim;
    private Vector3 _wristRotation;

    public Animator oppositeHandAnimator; // animator of opposite hand
    public Animator handAnimator;
    private GameObject _toInteractObj;  // check which object is it colliding with
    private InteractableObject _currObj;    // currently interacting with hand
    private bool _canInteract;  // can interact status

    [SerializeField] private GameObject grappleArmSpline;
    public bool left;
    
    private void Start()
    {
        _ogPosition = transform.localPosition;
        _wristRotation = Vector3.zero;
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

            // wrist rotation (done in LateUpdate)
            Vector2 lookMove = _lookAction.ReadValue<Vector2>();
            // yaw
            _wristRotation.x += lookMove.x * lookSensitivity * -1.0f;
            // pitch
            _wristRotation.y += lookMove.y * lookSensitivity;
            _wristRotation.y = Mathf.Clamp(_wristRotation.y, -90f, 90f);
            // roll
            _wristRotation.z += (_leftBumperAction.ReadValue<float>() * -1 + _rightBumperAction.ReadValue<float>()) * wristRotationSpeed;

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
            // left arm roll
            _wrist.localRotation = Quaternion.Euler(_wristRotation.y, _wristRotation.z, _wristRotation.x);
            _wristAim.localRotation = Quaternion.Euler(_wristRotation.y, _wristRotation.z, _wristRotation.x);
        }
        else
        {
            // right arm roll
            _wrist.localRotation = Quaternion.Euler(_wristRotation.y, _wristRotation.z * -1.0f, _wristRotation.x * -1.0f);
            _wristAim.localRotation = Quaternion.Euler(_wristRotation.y, _wristRotation.z * -1.0f, _wristRotation.x * -1.0f);
        }
    }


    // using TurnOn to initialize when player starts using the hand, not in Start() when object instantiate
    public void TurnOn(GameObject playerUsing)
    {
        _currPlayer = playerUsing;
        var input = _currPlayer.GetComponent<PlayerInput>();
        _moveAction = input.actions.FindAction("Move");
        _leftTriggerAction = input.actions.FindAction("LeftTrigger");
        _rightTriggerAction = input.actions.FindAction("RightTrigger");
        _leftBumperAction = input.actions.FindAction("LeftBumper");
        _rightBumperAction = input.actions.FindAction("RightBumper");
        _lookAction = input.actions.FindAction("Look");
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

