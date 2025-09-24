using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f;                             // movement speed
    public float pauseTime = 2f;                         // seconds to pause

    // offsets for elevator movement range
    public Vector3 upOffset = new Vector3(0, 5, 0);
    public Vector3 leftOffset = new Vector3(-5, 0, 0);
    public Vector3 downOffset = new Vector3(0, -5, 0);
    public Vector3 rightOffset = new Vector3(5, 0, 0);
    public int startIndex = 0; // not all elevator segments start at the same point
    public float startDelay = 0f; // delay so that we can have them not overlap
    public Transform startPoint; // used for initial tranform location for calculations
    private Vector3 startPosition;

    void Start()
    {
        startPosition = startPoint.position;
        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator()
    {
        // set up points of where to stop
        Vector3[] path = new Vector3[]
        {
            startPosition + upOffset,
            startPosition + upOffset + leftOffset,
            startPosition + leftOffset,
            startPosition
        };

        int index = startIndex; // start at one of the points

        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // get new target
            Vector3 target = path[index];

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
            index = (index + 1) % path.Length;
        }
    }
}
