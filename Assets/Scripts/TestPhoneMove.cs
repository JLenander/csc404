using UnityEngine;
using UnityEngine.InputSystem;

public class TestPhoneMove : MonoBehaviour
{
    public InputActionAsset input; // drag your TestControls asset here
    private InputAction moveAction;
    private InputAction rotateAction;
    
    public float moveSpeed = 5f;
    public float rotateSpeed = 50f;
    
    private void Awake()
    {
        var map = input.FindActionMap("PhoneControls");
        moveAction = map.FindAction("Move");
        rotateAction = map.FindAction("Rotate");
    }

    private void Update()
    {
        Vector3 moveInput = moveAction.ReadValue<Vector3>();
        Vector3 move = moveInput * (moveSpeed * Time.deltaTime);
        Vector2 rotateInput = rotateAction.ReadValue<Vector2>();
        transform.position += move;

        // Rotate phone along X (tilt) and Y (turn) axes
        if (rotateInput.sqrMagnitude > 0.01f)
        {
            float tilt = rotateInput.x * rotateSpeed * Time.deltaTime;
            float turn = rotateInput.y * rotateSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right * tilt, Space.Self);
            transform.Rotate(Vector3.up * turn, Space.World);
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
