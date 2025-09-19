using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneTestHandMove : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody _rb;

    public InputActionAsset input;
    private InputAction moveAction;
    public Vector3 movement;

    private bool _isMoving;
    public AudioSource moveSource;
    public AudioSource stopSource;
    
    private void Awake()
    {
        var map = input.FindActionMap("HandControls");
        moveAction = map.FindAction("Move");
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        movement = new Vector3(moveInput.x, moveInput.y, 0) * speed;
        bool movingNow = movement.magnitude > 0.5f;

        // Movement started
        if (movingNow && !_isMoving)
        {
            _isMoving = true;
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
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + movement);
    }
    
    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

}
