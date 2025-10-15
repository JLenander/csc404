using UnityEngine;

public class SceneExitDoor : MonoBehaviour
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other != null && other.CompareTag("Hand"))
        {
            // show scoreboard then exit scene after interact
            ScoreboardUIHandler.Instance.ShowScoreboard();
            triggered = true;
        }
    }
}
