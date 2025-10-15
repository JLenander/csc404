using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// a per level task initializer
public class TaskInitializer : MonoBehaviour
{
    [System.Serializable]
    public class TaskData
    {
        public TaskInfoSO taskInfoSO;
        public UnityEvent onStart;
        public UnityEvent onComplete;
    }
    public List<TaskData> tasksToRegister = new List<TaskData>();

    void Start()
    {
        foreach (var data in tasksToRegister)
        {
            TaskInfoSO taskInfo = data.taskInfoSO;

            var task = new Task
            {
                id = taskInfo.id,
                title = taskInfo.title,
                description = taskInfo.description,
                urgency = taskInfo.urgency,
                location = taskInfo.location,
                targetProgress = taskInfo.targetProgress,
                onStart = data.onStart,
                onComplete = data.onComplete
            };

            TaskManager.Instance.RegisterTask(task);
        }
    }
}
