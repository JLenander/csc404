using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    private InputAction moveAction;
    private InputAction dpadAction;
    public Vector3 movement;
    bool disable = false;

    private bool isMoving = false;
    public AudioSource moveSource;
    public AudioSource stopSource;

    private GameObject _currPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // audioSource = GetComponent<AudioSource>();

        // moveAction = InputSystem.actions.FindAction("Move");
        // moveAction.Enable();
        // dpadAction = InputSystem.actions.FindAction("DpadMove");
    }

    void Update()
    {
        if (disable)
        {
            // Vector2 move = dpadAction.ReadValue<Vector2>();
            // rb.linearVelocity = move * speed;

            Vector2 stickMove = moveAction.ReadValue<Vector2>();
            Vector2 dpadMove = dpadAction.ReadValue<Vector2>();
            Vector3 stickMovement = new Vector3(stickMove.x, stickMove.y, 0);
            Vector3 dpadMovement = new Vector3(0, 0, dpadMove.y) * -1;
            movement = (stickMovement + dpadMovement) * speed;
            rb.linearVelocity = movement;


            bool movingNow = movement.magnitude > 0.5f;

            // Movement started
            if (movingNow && !isMoving)
            {
                isMoving = true;

                if (moveSource != null && !moveSource.isPlaying)
                    moveSource.Play();
            }

            // Movement stopped
            if (!movingNow && isMoving)
            {
                isMoving = false;

                if (moveSource != null && moveSource.isPlaying)
                    moveSource.Stop();

                if (stopSource != null)
                    stopSource.Play();
            }
        }

    }

    public void TurnOn(GameObject playerUsing)
    {
        _currPlayer = playerUsing;
        var input = _currPlayer.GetComponent<PlayerInput>();
        moveAction = input.actions.FindAction("Move");
        dpadAction = input.actions.FindAction("DpadMove");
        disable = true;
    }

    public void TurnOff(GameObject playerUsing)
    {
        disable = false;
    }

    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     moveInput = context.ReadValue<Vector2>();
    //     Debug.Log("moving!");
    // }
    // private void FixedUpdate()
    // {
    //     rb.linearVelocity = movement * speed * Time.fixedDeltaTime;
    // }
}

