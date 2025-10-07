using UnityEngine;

public class TaskInitializer : MonoBehaviour
{
    public TaskInfoSO[] tasksToRegister;

    void Start()
    {
        foreach (var data in tasksToRegister)
        {
            var task = new Task
            {
                id = data.id,
                title = data.title,
                description = data.description,
                targetProgress = data.targetProgress
            };
            TaskManager.Instance.RegisterTask(task);
        }
    }
}
