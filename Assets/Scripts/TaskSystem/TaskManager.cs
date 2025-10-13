using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    private Dictionary<string, Task> _tasks = new Dictionary<string, Task>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
    }

    public void RegisterTask(Task task)
    {
        if (!_tasks.ContainsKey(task.id))
            _tasks.Add(task.id, task);
    }

    public Task GetTask(string id)
    {
        _tasks.TryGetValue(id, out var task);
        return task;
    }

    public void AddProgress(string id, int amount)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            if (!task.isActive)
                StartTask(id);
            task.AddProgress(amount);
        }
        else
            Debug.LogWarning($"Task '{id}' not found!");
    }

    public void StartTask(string id)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            task.canStart = true;
            task.StartTask();
        }
    }

    public List<Task> GetAllTasks()
    {
        return new List<Task>(_tasks.Values);
    }

    public List<Task> GetAllActiveTasks()
    {
        return _tasks.Values.Where(t => t.isActive).ToList();
    }
}
