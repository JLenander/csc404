using UnityEngine;

public class FingerTouch : MonoBehaviour
{
    private Vector3 lastPos;
    private bool inSwipeArea;
    private PhoneUIController phoneUI;

    private void Start()
    {
        // Find phone in scene (or assign manually in Inspector if multiple phones exist)
        phoneUI = FindObjectOfType<PhoneUIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PhoneButton"))
        {
            Debug.Log("Tapped app button!");
            phoneUI.ShowSwipe(); // open swipe screen
        }
        else if (other.CompareTag("SwipeArea"))
        {
            inSwipeArea = true;
            lastPos = transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (inSwipeArea && other.CompareTag("SwipeArea"))
        {
            Vector3 delta = transform.position - lastPos;

            if (Mathf.Abs(delta.x) > 0.05f) // threshold
            {
                if (delta.x > 0)
                {
                    Debug.Log("Swipe Right");
                    phoneUI.ShowMatch(); // show match
                }
                else
                {
                    Debug.Log("Swipe Left");
                    phoneUI.ShowSwipe(); // load next photo
                }

                inSwipeArea = false; // prevent double trigger
            }

            lastPos = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SwipeArea"))
        {
            inSwipeArea = false;
        }
    }
}