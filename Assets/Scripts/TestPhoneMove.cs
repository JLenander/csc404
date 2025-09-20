using UnityEngine;
using UnityEngine.InputSystem;

public class TestPhoneMove : MonoBehaviour
{
    public InputActionAsset input; // TestControls asset here
    private InputAction _moveAction;
    private InputAction _rotateAction;
    
    public float moveSpeed = 5f;
    public float rotateSpeed = 50f;
    
    private Rigidbody _rb;
    
    // cache inputs read in Update, apply in FixedUpdate
    private Vector3 _moveInput;
    private Vector2 _rotateInput;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var map = input.FindActionMap("PhoneControls");
        _moveAction = map.FindAction("Move");
        _rotateAction = map.FindAction("Rotate");
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector3>();
        _rotateInput = _rotateAction.ReadValue<Vector2>();
    }
    
    private void FixedUpdate()
    {
        // Move using Rigidbody.MovePosition
        // Flip x-axis to match hand coord system - relative to player perspective
        Vector3 move = new Vector3(-_moveInput.x, _moveInput.y, _moveInput.z) * (moveSpeed * Time.fixedDeltaTime);
        Vector3 targetPos = _rb.position + move;
        _rb.MovePosition(targetPos);

        // Rotate using Rigidbody.MoveRotation
        if (_rotateInput.sqrMagnitude > 0.01f)
        {
            // Flip y to tilt is towards input
            float tilt = -_rotateInput.y * rotateSpeed * Time.fixedDeltaTime;
            // Keep flipped x so turn is towards input
            float turn = _rotateInput.x * rotateSpeed * Time.fixedDeltaTime;

            // apply local X tilt, then global Y turn
            Quaternion localTilt = Quaternion.Euler(tilt, 0f, turn);
            // Quaternion worldTurn = Quaternion.Euler(0f, turn, 0f); -- for a world axis turn

            Quaternion newRot = _rb.rotation * localTilt;      // local tilt
            // newRot = worldTurn * newRot;                    // then global turn

            _rb.MoveRotation(newRot);
        }
    }
    
    private void OnEnable()
    {
        _moveAction.Enable();
        _rotateAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _rotateAction.Disable();
    }

}
