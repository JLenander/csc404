using UnityEngine;

/// <summary>
/// If a player enters this object's trigger zone, tp it to this object's parent position.
/// Originally used in the segments in the interior corridors
/// </summary>
public class TPtoParent : MonoBehaviour
{
    private Transform _parent;
    void Start()
    {
        _parent = gameObject.transform.parent;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Caching this on Start actually worsens performance as these segments may be removed or added based on spline
            // length and we hope to very rarely if ever have this trigger be hit.
            var controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            Debug.Log("Segment Trigger entered. Moving to position " + _parent.position + " from current " + other.transform.position);
            other.transform.position = _parent.position;
            controller.enabled = true;
        }
    }
}
