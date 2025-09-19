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
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float rotateInput = rotateAction.ReadValue<float>();

        // Move phone using Transform
        Vector3 move = new Vector3(-1 * moveInput.x, moveInput.y, 0) * (moveSpeed * Time.deltaTime);
        transform.position += move;

        // Rotate phone along X axis (tilt)
        if (Mathf.Abs(rotateInput) > 0.01f)
        {
            transform.Rotate(Vector3.right * (rotateInput * rotateSpeed * Time.deltaTime));
        }
    }
}
