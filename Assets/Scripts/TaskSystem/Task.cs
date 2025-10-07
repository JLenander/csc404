using System;
using UnityEngine;

[Serializable]
public class Task
{
    public string id;
    public string title;
    [TextArea] public string description;

    public int currentProgress;
    public int targetProgress = 1;

    public bool canStart;
    public bool started;

    public bool isCompleted => currentProgress >= targetProgress;

    public event Action<Task> OnProgressUpdated;
    public event Action<Task> OnCompleted;
    public event Action<Task> OnStarted;

    // start task only if it can start and not completed
    public void StartTask()
    {
        if (isCompleted)
        {
            Debug.LogWarning($"Task '{id}' already completed!");
            return;
        }
         
        if (!canStart)
        {
            Debug.LogWarning($"Task '{id}' cannot start yet!");
            return;
        }

        currentProgress = 0;
        started = true;
        Debug.Log($"Task '{id}' started!");
        OnStarted?.Invoke(this);
    }

    // Add progress (only if started and not already complete)
    public void AddProgress(int amount)
    {
        if (!canStart)
        {
            Debug.LogWarning($"Task '{id}' can't progress because it hasn't started.");
            return;
        }

        if (isCompleted)
        {
            Debug.Log($"Task '{id}' is already completed!");
            return;
        }
        currentProgress = Mathf.Min(currentProgress + amount, targetProgress);
        Debug.Log(currentProgress);
        OnProgressUpdated?.Invoke(this);

        if (isCompleted)
        {
            Debug.Log($"Task '{id}' completed!");
            OnCompleted?.Invoke(this);
        }
    }

    public void ResetTask()
    {
        currentProgress = 0;
        canStart = false;
    }
}
