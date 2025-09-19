using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.AnimationPlayableAsset;

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

    public float lookSensitivity = 0.4f;

    private Transform _wrist;
    private float wristRotateX;
    private float wristRotateY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wrist = _rb.gameObject.transform.parent.parent.Find("jt_top/jt_mid/jt_bot");
    }

    private void Update()
    {
        if (_disable)
        {
            // hand rigid body movement
            Vector2 stickMove = _moveAction.ReadValue<Vector2>();
            Vector2 dpadMove = _dpadAction.ReadValue<Vector2>();
            Vector3 stickMovement = new Vector3(-1 * stickMove.x, stickMove.y, 0);
            Vector3 dpadMovement = new Vector3(0, 0, dpadMove.y) * -1;
            movement = (stickMovement + dpadMovement) * speed;
            // movement done in FixedUpdate

            // rotation movement (done in LateUpdate)
            Vector2 lookMove = _lookAction.ReadValue<Vector2>();
            wristRotateX += lookMove.x * lookSensitivity * -1.0f;
            wristRotateY += lookMove.y * lookSensitivity;
            wristRotateY = Mathf.Clamp(wristRotateY, -90f, 90f);

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

    private void LateUpdate()
    {
        _wrist.localRotation *= Quaternion.Euler(wristRotateY, wristRotateX, 0);
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

