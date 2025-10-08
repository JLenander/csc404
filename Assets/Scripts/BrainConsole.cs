using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class BrainConsole : Interactable
{
    private bool _canInteract = true;
    public GameObject playerTaskPanel; 
    public TMPro.TextMeshPro tasksList; 

    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        _canInteract = false;

        playerTaskPanel.SetActive(true);
        UpdateTaskList();
    }
    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        playerTaskPanel.SetActive(false);
        _canInteract = true;
    }

    public override bool CanInteract()
    {
        return _canInteract;
    }

    private void UpdateTaskList()
    {
        // Get all active tasks for the player
        var activeTasks = TaskManager.Instance.GetAllActiveTasks();

        if (activeTasks.Count == 0)
        {
            tasksList.text = "No active tasks";
            return;
        }

        string taskText = "";
        foreach (var task in activeTasks)
        {
            taskText += $"{task.title}: {task.currentProgress}/{task.targetProgress}\n";
        }

        tasksList.text = taskText;
    }
}
