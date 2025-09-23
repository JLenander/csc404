using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f;                             // movement speed
    public float pauseTime = 2f;                         // seconds to pause

    public Vector3 upOffset = new Vector3(0, 5, 0);
    public Vector3 leftOffset = new Vector3(-5, 0, 0);
    public Vector3 downOffset = new Vector3(0, -5, 0);
    public Vector3 rightOffset = new Vector3(5, 0, 0);
    public int startIndex = 0;

    public float startDelay = 0f;
    public Transform startPoint;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = startPoint.position;
        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator()
    {
        Vector3[] path = new Vector3[]
        {
            startPosition + upOffset,
            startPosition + upOffset + leftOffset,
            startPosition + leftOffset,
            startPosition
        };

        int index = startIndex;

        yield return new WaitForSeconds(startDelay);

        while (true)
        {
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
