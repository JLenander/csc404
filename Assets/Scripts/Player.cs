using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // ID of player
    // public int playerID;
    public float moveSpeed;
    public float lookSensitivity;

    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f;
    private CharacterController _characterController;
    private Camera _playerCamera;
    private Camera _outsideCamera;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private float xRotation = 0f;
    private float yRotation = 0f; // left/right (yaw)
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;

    private bool disableMovement = false;
    private bool disableRotate = false;

    private delegate void ControlFunc();
    private ControlFunc _controlFunc;

    private bool controllingEyeCam = false;
    private bool controllingRobot = false;
    private Animator animator;
    
    private float stepTimer;

    private RobotMovement _robotMovement;
    
    private readonly Color[] _playerColors = {
        Color.red,      // Player 1
        Color.blue,     // Player 2
        Color.yellow,   // Player 3
    };

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        var input = GetComponent<PlayerInput>();
        _playerCamera = input.camera;
        _moveAction = input.actions.FindAction("Move");
        _lookAction = input.actions.FindAction("Look");
        animator = GetComponentInChildren<Animator>();
        _characterController.enabled = false;
        this.transform.position = new Vector3(-1.0f, 5.0f, -3.0f);
        _characterController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        
        var index = input.playerIndex;
        var outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineColor = _playerColors[index];
        }

        _controlFunc = ControlPlayer;
    }

    void FixedUpdate()
    {
        _controlFunc();
        //if (controllingEyeCam)
        //{
        //    ControlEyeCam();
        //}
        //else if (controllingRobot) {
        //    _robotMovement.ControlRobotMovement();
        //}
        //else
        //{
        //    ControlPlayer();
        //}
    }

    private void ControlPlayer()
    {
        if (!disableMovement)
        {
            isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
            // Movement

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // small downward force to keep grounded
            }

            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            // Camera directions (ignore vertical tilt)
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0;
            right.Normalize();

            // Combine input with camera directions
            Vector3 moveDir = (forward * moveValue.y + right * moveValue.x).normalized;
            _characterController.Move(moveDir * moveSpeed * Time.deltaTime);

            velocity.y += gravity;
            _characterController.Move(velocity * Time.deltaTime);

            float speed = moveDir.magnitude;
            animator.SetFloat("Speed", speed);

            if (speed != 0)
            {
                stepTimer -= Time.fixedDeltaTime;
                if (stepTimer <= 0f)
                {
                    PlayFootstep();
                    stepTimer = stepInterval;
                }
            }
        }
        
        if (!disableRotate)
        {
            // Look
            Vector2 lookValue = _lookAction.ReadValue<Vector2>();
            Vector3 lookRotate = new Vector3(0, lookValue.x * lookSensitivity * -1, 0);
            xRotation -= lookValue.y * lookSensitivity;
            yRotation -= lookValue.x * lookSensitivity * -1;
            
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            xRotation = Math.Clamp(xRotation, -90f, 90f);
            _playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
    
    private void ControlEyeCam()
    {
        // Look
        Vector2 lookValue = _lookAction.ReadValue<Vector2>();
        Vector3 lookRotate = new Vector3(0, lookValue.x * lookSensitivity * -1, 0);
        xRotation -= lookValue.y * lookSensitivity;
        yRotation -= lookValue.x * lookSensitivity * -1;
        
        yRotation = Math.Clamp(yRotation, -30f, 30f);
        xRotation = Math.Clamp(xRotation, -70f, 70f);
        _outsideCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, footstepClips.Length);
            footstepSource.PlayOneShot(footstepClips[index]);
        }
    }

    public void TurnOff()
    {
        disableMovement = true;
        disableRotate = true;
    }

    public void TurnOn()
    {
        disableMovement = false;
        disableRotate = false;
    }

    public void switchToHead(Camera outsideCamera)
    {
        disableMovement = true;
        disableRotate = false;
        controllingEyeCam = true;
        _outsideCamera = outsideCamera;
        _controlFunc = ControlEyeCam;
    }

    public void switchOffHead()
    {
        disableMovement = false;
        disableRotate = false;
        controllingEyeCam = false;
        _controlFunc = ControlPlayer;
    }

    public void switchToLegs(Transform robotBody)
    {
        disableMovement = true;
        disableRotate = false;
        controllingRobot = true;
        _robotMovement = robotBody.GetComponent<RobotMovement>();
        _robotMovement.SetMoveAction(_moveAction);
        _robotMovement.SetLookAction(_lookAction);
        _controlFunc = _robotMovement.ControlRobotMovement;
    }

    public void switchOffLegs()
    {
        disableMovement = false;
        disableRotate = false;
        controllingRobot = false;
        _robotMovement = null;

        _controlFunc = ControlPlayer;
    }
}
