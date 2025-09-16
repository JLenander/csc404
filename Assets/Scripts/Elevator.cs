using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Vector3 topPosition = new Vector3(0, 5, 0);   // local offset for top
    public float speed = 2f;                             // movement speed
    public float pauseTime = 2f;                         // seconds to pause

    private Vector3 bottomPosition;
    private bool goingUp = true;

    void Start()
    {
        bottomPosition = transform.position; // starting position is bottom
        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator()
    {
        while (true)
        {
            Vector3 target = goingUp ? bottomPosition + topPosition : bottomPosition;

            // Move towards target
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            // Snap exactly to target to avoid floating-point drift
            transform.position = target;

            // Pause at target
            yield return new WaitForSeconds(pauseTime);

            // Switch direction
            goingUp = !goingUp;
        }
    }
}
