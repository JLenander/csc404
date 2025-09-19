using UnityEngine;
using UnityEngine.InputSystem;

public class HandMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody _rb;

    private InputAction _moveAction;
    private InputAction _dpadAction;
    private InputAction _lookAction;
    public Vector3 movement;
    private bool _disable;

    private bool _isMoving;
    public AudioSource moveSource;
    public AudioSource stopSource;

    private GameObject _currPlayer;

    public float lookSensitivity = 2.5f;

    public GameObject rootBone;
    private Transform _wrist;
    private Transform _top;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wrist = rootBone.transform.Find("jt_top/jt_mid/jt_bot");
        _top = rootBone.transform.Find("jt_top");

        Debug.Log(rootBone);
        Debug.Log(_top);
    }

    private void Update()
    {
        if (_disable)
        {
            Vector2 stickMove = _moveAction.ReadValue<Vector2>();
            Vector2 dpadMove = _dpadAction.ReadValue<Vector2>();
            Vector2 lookMove = _lookAction.ReadValue<Vector2>();
            Vector3 stickMovement = new Vector3(-1 * stickMove.x, stickMove.y, 0);
            Vector3 dpadMovement = new Vector3(0, 0, dpadMove.y) * -1;
            movement = (stickMovement + dpadMovement) * speed;

            _wrist.Rotate(Vector3.right, lookMove.y * lookSensitivity * Time.deltaTime);


            // movement done in FixedUpdate

            bool movingNow = movement.magnitude > 0.5f;

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
        }

    }

    // Use FixedUpdate for physics-based movement
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + movement * Time.fixedDeltaTime);
    }

    
    // using TurnOn to initialize when player starts using the hand, not in Start() when object instantiate
    public void TurnOn(GameObject playerUsing)
    {
        _currPlayer = playerUsing;
        var input = _currPlayer.GetComponent<PlayerInput>();
        _moveAction = input.actions.FindAction("Move");
        _dpadAction = input.actions.FindAction("DpadMove");
        _lookAction = input.actions.FindAction("Look");
        _disable = true;
    }

    public void TurnOff(GameObject playerUsing)
    {
        _disable = false;
    }
    
}

