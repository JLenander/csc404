using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class RobotMovement : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _lookAction;

    private CharacterController _robotCharacterController;
    private Vector3 _robotVelocity;
    private bool _robotIsGrounded;
    public float robotMoveSpeed = 50f;
    public float robotLookSensitivity = 50f;
    public bool disable = false;

    // [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        var input = GetComponent<PlayerInput>();
        _robotCharacterController = GetComponentInChildren<CharacterController>();

        if (_robotCharacterController == null)
            Debug.LogError("No CharacterController found");
    }

    public void ControlRobotMovement()
    {
        if (disable) return;
        _robotIsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        // Movement

        if (_robotIsGrounded && _robotVelocity.y < 0)
        {
            _robotVelocity.y = -2f; // small downward force to keep grounded
        }
        _robotVelocity.y += gravity;

        float leftInput = _moveAction.ReadValue<Vector2>().y;
        float rightInput = _lookAction.ReadValue<Vector2>().y;

        if (Mathf.Abs(leftInput) < 0.1f) leftInput = 0;
        if (Mathf.Abs(rightInput) < 0.1f) rightInput = 0;

        float moveInput = (leftInput + rightInput) / 2f;
        Vector3 moveDir = transform.forward * moveInput + _robotVelocity;
        _robotCharacterController.Move(moveDir * robotMoveSpeed * Time.deltaTime);

        float rotateInput = (leftInput - rightInput);
        transform.Rotate(Vector3.up, rotateInput * robotLookSensitivity * Time.deltaTime);

        if (Mathf.Abs(moveInput) > 0 || Mathf.Abs(rotateInput) > 0)
        {
            GlobalPlayerUIManager.Instance.StartWalkingShake();
            stepTimer -= Time.fixedDeltaTime;
            if (stepTimer <= 0f)
            {
                // PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            GlobalPlayerUIManager.Instance.StopWalkingShake();
        }
    }

    // public void PlayFootstep()
    // {
    //     if (footstepClips.Length > 0)
    //     {
    //         int index = UnityEngine.Random.Range(0, footstepClips.Length);
    //         footstepSource.PlayOneShot(footstepClips[index]);
    //     }
    // }

    public void SetMoveAction(InputAction moveAction)
    { _moveAction = moveAction; }

    public void SetLookAction(InputAction lookAction)
    { _lookAction = lookAction; }

}
