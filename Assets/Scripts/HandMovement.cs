using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("lookSensitivity")] public float handPitchYawSensitivity = 0.4f;

    [SerializeField] private float wristRotationSpeed = 1.0f;

    // The transforms to control the hand/wrist roll/pitch/yaw (airplane degrees of freedom system).
    // Pitch and Yaw are separate from Roll as we want them to be independent of the hand/wrist roll orientation.
    [FormerlySerializedAs("_wrist")] [SerializeField] private Transform _wristRoll;
    [SerializeField] private Transform _wristPitchYaw;
    [SerializeField] private Transform _wristAim;
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
        if (!_disable)
        {
            // hand rigid body movement
            Vector2 stickMove = _moveAction.ReadValue<Vector2>();
            Vector3 stickMovement = new Vector3(stickMove.x, stickMove.y, 0);
            
            float leftTrigger = _leftTriggerAction.ReadValue<float>();
            float rightTrigger = _rightTriggerAction.ReadValue<float>();
            Vector3 triggerMovement = new Vector3(0, 0, leftTrigger - rightTrigger);
            
            movement += (stickMovement + triggerMovement) * Time.deltaTime;
            // movement done in FixedUpdate

            // wrist rotation (done in FixedUpdate)
            Vector2 lookMove = _lookAction.ReadValue<Vector2>() * Time.deltaTime;
            // yaw
            _wristRotation.x += lookMove.x * handPitchYawSensitivity;
            // pitch
            _wristRotation.y += lookMove.y * handPitchYawSensitivity;
            // roll
            _wristRotation.z += (_leftBumperAction.ReadValue<float>() * -1 + _rightBumperAction.ReadValue<float>()) * wristRotationSpeed * Time.deltaTime;
            ClampWristRotate();
            
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
        // Movement
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
        
        // Rotation
        // pitch and yaw on parent object so the direction is independent of the wrist roll orientation.
        if (left)
        {
            // left hand pitch and yaw
            _wristPitchYaw.localRotation = Quaternion.Euler(_wristRotation.y, 0, _wristRotation.x * -1.0f);
            _wristAim.localRotation = Quaternion.Euler(_wristRotation.y, 0, _wristRotation.x * -1.0f);
            // left hand roll
            _wristRoll.localRotation = Quaternion.Euler(0, _wristRotation.z, 0);
        }
        else
        {
            // right hand pitch and yaw
            _wristPitchYaw.localRotation = Quaternion.Euler(_wristRotation.y, 0, _wristRotation.x);
            _wristAim.localRotation = Quaternion.Euler(_wristRotation.y, 0, _wristRotation.x);
            // right hand roll
            _wristRoll.localRotation = Quaternion.Euler(0, _wristRotation.z * -1.0f, 0);
        }
    }

    /// <summary>
    /// Clamp the wrist rotate to the appropriate values to prevent excessive wrist rotation
    /// </summary>
    private void ClampWristRotate()
    {
        _wristRotation.x = Mathf.Clamp(_wristRotation.x, -90f, 90f);
        _wristRotation.y = Mathf.Clamp(_wristRotation.y, -90f, 90f);
    }

    public Vector3 GetWristRotation()
    {
        return _wristRotation;
    }

    public void SetWristRotation(Vector3 wristRotation)
    {
        _wristRotation = wristRotation;
        ClampWristRotate();
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
        _disable = false;
    }

    public void TurnOff(GameObject playerUsing)
    {
        _disable = true;
    }

    public void SetCurrentInteractableObject(GameObject handUsing, bool canInteract)
    {
        _toInteractObj = handUsing;
        _canInteract = canInteract;
    }

    private void InteractWithObject(InteractableObject interactableObject)
    {
        Debug.Log("Interacting with " + interactableObject);
        interactableObject.InteractWithHand(_wristRoll, this);
    }

    public void StopInteractingWithObject(InteractableObject interactableObject)
    {
        Debug.Log("Stopping interaction with " + interactableObject);
        interactableObject.StopInteractWithHand(this);
        _currObj = null;
    }

}

