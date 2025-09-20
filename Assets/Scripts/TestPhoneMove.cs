using UnityEngine;
using UnityEngine.InputSystem;

public class TestPhoneMove : MonoBehaviour
{
    public InputActionAsset input; // drag your TestControls asset here
    private InputAction moveAction;
    private InputAction rotateAction;
    
    public float moveSpeed = 5f;
    public float rotateSpeed = 50f;
    
    private Rigidbody rb;
    
    // cache inputs read in Update, apply in FixedUpdate
    private Vector3 moveInput;
    private Vector2 rotateInput;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        var map = input.FindActionMap("PhoneControls");
        moveAction = map.FindAction("Move");
        rotateAction = map.FindAction("Rotate");
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector3>();
        rotateInput = rotateAction.ReadValue<Vector2>();
    }
    
    private void FixedUpdate()
    {
        // Move using Rigidbody.MovePosition
        // Flip x-axis to match hand coord system - relative to player perspective
        Vector3 move = new Vector3(-moveInput.x, moveInput.y, moveInput.z) * (moveSpeed * Time.fixedDeltaTime);
        Vector3 targetPos = rb.position + move;
        rb.MovePosition(targetPos);

        // Rotate using Rigidbody.MoveRotation
        if (rotateInput.sqrMagnitude > 0.01f)
        {
            float tilt = rotateInput.y * rotateSpeed * Time.fixedDeltaTime;
            // Flip x to match player perspective
            float turn = -rotateInput.x * rotateSpeed * Time.fixedDeltaTime;

            // apply local X tilt, then global Y turn
            Quaternion localTilt = Quaternion.Euler(tilt, 0f, 0f);
            Quaternion worldTurn = Quaternion.Euler(0f, turn, 0f);

            Quaternion newRot = rb.rotation * localTilt;      // local tilt
            newRot = worldTurn * newRot;                      // then global turn

            rb.MoveRotation(newRot);
        }
    }
    
    private void OnEnable()
    {
        moveAction.Enable();
        rotateAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        rotateAction.Disable();
    }

}
