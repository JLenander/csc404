using UnityEngine;

public class TriggerSeat : MonoBehaviour
{
    public NovaLevel1Manager novaLevel1Manager;
    public CharacterController robotCharController;
    public RobotMovement robotMovement;
    public Transform robot;
    public SceneExitDoor sceneExitDoor;

    private bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // trigger level start at trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other != null && other.CompareTag("Hand"))
        {
            novaLevel1Manager.PlayLevelRoutine();
            robotMovement.disable = true;
            SeatRobot(new Vector3(253.3f, 18.1f, 60.1f));
            triggered = true;
        }
    }

    void SeatRobot(Vector3 position)
    {
        robotCharController.enabled = false;
        robot.position = position;
        robot.rotation = new Quaternion(0, 180, 0, 0);
        GlobalPlayerUIManager.Instance.StopWalkingShake();
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    public void StandRobot()
    {
        robotCharController.enabled = true;
        robotMovement.disable = false;
        // enable the exit door collier
        sceneExitDoor.enabled = true;
    }
}
