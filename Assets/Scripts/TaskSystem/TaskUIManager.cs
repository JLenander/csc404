using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public GameObject taskPrefab; // TaskUIPrefab
    private Dictionary<string, GameObject> activeTaskUIs = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        UpdateTaskUI();
        SubscribeToTasks();
    }

    private void OnDisable()
    {
        UnsubscribeFromTasks();
    }

    void SubscribeToTasks()
    {
        foreach (var task in TaskManager.Instance.GetAllTasks())
        {
            task.OnProgressUpdated += (_) => UpdateTaskUI();
            task.OnStarted += (_) => UpdateTaskUI();
            task.OnCompleted += (_) => UpdateTaskUI();
        }
    }

    void UnsubscribeFromTasks()
    {
        foreach (var task in TaskManager.Instance.GetAllTasks())
        {
            task.OnProgressUpdated -= (_) => UpdateTaskUI();
            task.OnStarted -= (_) => UpdateTaskUI();
            task.OnCompleted -= (_) => UpdateTaskUI();
        }
    }

    public void UpdateTaskUI()
    {
        var activeTasks = TaskManager.Instance.GetAllActiveTasks();

        // Remove old UIs
        List<string> keysToRemove = new List<string>();
        foreach (var kvp in activeTaskUIs)
        {
            if (!activeTasks.Exists(t => t.id == kvp.Key))
            {
                Destroy(kvp.Value);
                keysToRemove.Add(kvp.Key);
            }
        }
        foreach (var key in keysToRemove)
            activeTaskUIs.Remove(key);

        // Add/update UIs
        foreach (var task in activeTasks)
        {
            if (!activeTaskUIs.ContainsKey(task.id))
            {
                GameObject taskUI = Instantiate(taskPrefab, transform);
                activeTaskUIs.Add(task.id, taskUI);
            }

            var textComponent = activeTaskUIs[task.id].GetComponent<TMP_Text>(); // or Text
            textComponent.text = $"{task.title}: {task.currentProgress}/{task.targetProgress}";
        }
    }
}
