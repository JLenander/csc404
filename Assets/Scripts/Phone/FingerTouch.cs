// Handles finger touch input for phone screen interaction

using UnityEngine;

public class FingerTouch : MonoBehaviour
{
    public GameObject phone;                // Phone prefab
    // public Transform finger;              // fingertip for position
    // refactored to use built-in "this" GameObject's "transform" for position instead of separate public finger
    private Transform _swipeArea;           // SwipeArea for position

    private readonly float _swipeThreshold = 0.01f;  // world units in local X, for distance to validate a "swipe"
    // (0.02 was too far, 0.01 can be too short, try 0.015 next time)  (CHANGEABLE)
    private bool _inSwipeArea;
    private Vector3 _startLocalPos;
    private PhoneUIController _phoneUI;

    [SerializeField] private Transform neutralPose; // empty in the scene that marks default position/rotation
    [SerializeField] private float returnSpeed = 5f; // how fast it returns
    [SerializeField] private bool returning = true; // toggle externally if needed
    private Rigidbody _rb;

    private void Start()
    {
        // can do FindObjectOfType but deprecated, so find through phone Object
        _phoneUI = phone.GetComponent<PhoneUIController>();
        // Using swipeArea axis so movement in swipeArea's perspective
        // should be able to work with phone but this way more linked to the actual collider we want to work with
        _swipeArea = phone.transform.Find("SwipeArea");
        if (_swipeArea == null)
            Debug.LogError("SwipeArea not found under phone!");

        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!returning || neutralPose == null) return;

        Vector3 toTarget = neutralPose.position - _rb.position;
        float distance = toTarget.magnitude;

        if (distance < 0.1f) // threshold to stop jitter
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 direction = toTarget.normalized;
        _rb.linearVelocity = direction * Mathf.Min(returnSpeed, distance / Time.fixedDeltaTime);
    }

    // Detect entering button or swipe area, fires once only
    // Hence for button press, only need OnTriggerEnter
    // doesn't matter finger stays on screen because OnTriggerEnter won't fire again until finger exits and re-enters
    // For swipe, need OnTriggerStay to track movement while inside area and OnTriggerExit to reset state
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SwipeArea"))
        {
            _inSwipeArea = true;
            Debug.Log($"Entered swipe area");
            // Convert finger world pos to swipeArea local space
            // Recall this transform is of the attached finger touch object
            _startLocalPos = _swipeArea.InverseTransformPoint(transform.position);
        }
        else if (other.CompareTag("PhoneButton"))
        {
            Debug.Log("Tapped app button");
            other.enabled = false;
            _phoneUI.ShowSwipe();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // ignores non‑SwipeArea colliders and prevents processing when already cleared _inSwipeArea
        if (!_inSwipeArea || !other.CompareTag("SwipeArea"))
            return;

        // Get current finger pos in swipeArea local space
        // Recall this transform is of the attached finger touch object
        Vector3 localFingerPos = _swipeArea.InverseTransformPoint(transform.position);
        // Calc local X traveled since entering area
        float deltaX = localFingerPos.x - _startLocalPos.x;

        // Check if traveled far enough to count as a swipe
        if (Mathf.Abs(deltaX) > _swipeThreshold)
        {
            // delta is relative to swipeArea local X axis, so <0 is right swipe, >0 is left swipe
            if (deltaX < 0)
            {
                Debug.Log("Swipe Right");
                _phoneUI.ShowSwipe();
            }
            else
            {
                Debug.Log("Swipe Left");
                _phoneUI.ShowMatch();
            }
            _inSwipeArea = false; // stop multiple swipes
        }
    }

    // Reset state when finger exits swipe area, in case not a left/right swipe
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SwipeArea"))
        {
            _inSwipeArea = false;
            Debug.Log("Exited swipe area");
        }
    }
}
