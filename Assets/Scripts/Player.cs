using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // ID of player
    // public int playerID;
    public float moveSpeed;
    public float moveSensitivity;
    public float lookSensitivity;

    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f;
    private CharacterController _characterController;
    private Camera _camera;
    private Transform _cameraTransform;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private float xRotation = 0f;
    private float yRotation = 0f; // left/right (yaw)
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;

    private bool disable = false;
    private bool outOfBody = false;
    private Animator animator;

    private float stepTimer;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        var input = GetComponent<PlayerInput>();
        _camera = input.camera;
        _cameraTransform = _camera.GetComponent<Transform>();
        _moveAction = input.actions.FindAction("Move");
        _lookAction = input.actions.FindAction("Look");
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (!disable)
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

        // Look
        Vector2 lookValue = _lookAction.ReadValue<Vector2>();
        Vector3 lookRotate = new Vector3(0, lookValue.x * lookSensitivity * -1, 0);
        xRotation -= lookValue.y * lookSensitivity;
        yRotation -= lookValue.x * lookSensitivity * -1;
        // xRotation = Math.Clamp(xRotation, -90f, 90f);


        if (outOfBody)
        {
            yRotation = Math.Clamp(yRotation, -30f, 30f);
            xRotation = Math.Clamp(xRotation, -70f, 70f);
            _camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            xRotation = Math.Clamp(xRotation, -90f, 90f);
            _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

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
        disable = true;
    }

    public void TurnOn()
    {
        disable = false;
    }

    public void switchToHead()
    {
        outOfBody = true;
    }

    public void switchOffHead()
    {
        outOfBody = false;
    }
}
