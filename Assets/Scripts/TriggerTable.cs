using UnityEngine;

public class TriggerTable : MonoBehaviour
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other != null && other.CompareTag("Hand"))
        {
            TaskManager.Instance.CompleteTask("GoPhone");
            TaskManager.Instance.StartTask("Pickup");
            triggered = true;
        }
    }
}
