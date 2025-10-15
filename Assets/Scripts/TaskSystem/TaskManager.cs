using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    BrainUIHandler uIHandler;

    private Dictionary<string, Task> _tasks = new Dictionary<string, Task>();

    private List<Task> activeTasks = new List<Task>(); // a list of active tasks ordered chronologically

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaitForBrainUIHandler());
    }

    IEnumerator WaitForBrainUIHandler()
    {
        yield return new WaitUntil(() => BrainUIHandler.Instance != null);
        uIHandler = BrainUIHandler.Instance;
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

    // public void AddProgress(string id, int amount)
    // {
    //     if (_tasks.TryGetValue(id, out var task))
    //     {
    //         if (!task.isActive)
    //             StartTask(id);
    //         task.AddProgress(amount);
    //     }
    //     else
    //         Debug.LogWarning($"Task '{id}' not found!");
    // }

    public void StartTask(string id)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            task.canStart = true;
            task.StartTask();
        }
    }

    public void CompleteTask(string id)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            task.CompleteTask();
        }
    }

    public void ResetTask(string id)
    {
        if (_tasks.TryGetValue(id, out var task))
        {
            task.ResetTask();
        }
    }

    public void AppendActiveTask(Task task)
    {
        activeTasks.Add(task);
        PassDataUI();
        if (task.id != "FixRight" && task.id != "FixLeft")
        {
            PopUpUIHandler.Instance.ShowPopUp();
        }
    }

    public void RemoveActiveTask(Task task)
    {
        activeTasks.Remove(task);
        PassDataUI();
    }

    // called whenever active tasks are changed
    private void PassDataUI()
    {
        List<string> taskNames = new List<string>();

        foreach (Task task in activeTasks)
        {
            taskNames.Add(task.title);
        }

        uIHandler.UpdateTasks(taskNames);
    }

    public Task TaskData(string name)
    {
        // get data of active task from name
        foreach (Task task in activeTasks)
        {
            if (task.title == name)
            {
                return task;
            }
        }

        return null;
    }

    public List<Task> GetActiveTasks()
    {
        return activeTasks;
    }

    // public List<Task> GetAllTasks()
    // {
    //     return new List<Task>(_tasks.Values);
    // }

    // public List<Task> GetAllActiveTasks()
    // {
    //     return _tasks.Values.Where(t => t.isActive).ToList();
    // }
}
