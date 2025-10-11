using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 2f;                             // movement speed
    public float pauseTime = 2f;                         // seconds to pause
    public float moveDistance = 5f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingUp = true;
    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, moveDistance, 0);
        StartCoroutine(MoveElevator());
    }

    private IEnumerator MoveElevator()
    {
        while (true)
        {
            Vector3 target = movingUp ? endPos : startPos;

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }

            // Snap to target just in case
            transform.position = target;

            // Pause at top/bottom
            yield return new WaitForSeconds(pauseTime);

            // Switch direction
            movingUp = !movingUp;
        }
    }
}
