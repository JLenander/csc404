using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneTestHandMove : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody _rb;

    private InputAction _moveAction;
    private InputAction _dpadAction;
    public Vector3 movement;

    private bool _isMoving;
    public AudioSource moveSource;
    public AudioSource stopSource;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _dpadAction = InputSystem.actions.FindAction("DpadMove");
    }

    private void Update()
    {
        Vector2 stickMove = _moveAction.ReadValue<Vector2>();
        Vector2 dpadMove = _dpadAction.ReadValue<Vector2>();
        Vector3 stickMovement = new Vector3(-1 * stickMove.x, stickMove.y, 0);
        Vector3 dpadMovement = new Vector3(0, 0, dpadMove.y) * -1;
        movement = (stickMovement + dpadMovement) * speed;
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
        _rb.MovePosition(_rb.position + movement * Time.fixedDeltaTime);
    }
}
