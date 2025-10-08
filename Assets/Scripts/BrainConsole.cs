using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class BrainConsole : Interactable
{
    private bool _canInteract = true;
    [SerializeField] GameObject leftDoorObj;
    [SerializeField] GameObject rightDoorObj;

    [SerializeField] GameObject leftEscapeDoorObj;
    [SerializeField] GameObject rightEscapeDoorObj;

    private Door leftDoor, rightDoor, leftEscapeDoor, rightEscapeDoor;
    private InputAction _leftTriggerAction, _rightTriggerAction;

    //public GameObject playerTaskPanel; 
    //public TMPro.TextMeshPro tasksList; 

    private void Start()
    {
        DisableOutline();
        leftDoor = leftDoorObj.GetComponent<Door>();
        rightDoor = rightDoorObj.GetComponent<Door>();
        leftEscapeDoor = leftEscapeDoorObj.GetComponent<Door>();
        rightEscapeDoor = rightEscapeDoorObj.GetComponent<Door>();
    }

    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        _canInteract = false;

        var input = player.GetComponent<PlayerInput>();
        _leftTriggerAction = input.actions.FindAction("LeftTrigger");
        _rightTriggerAction = input.actions.FindAction("RightTrigger");

        //playerTaskPanel.SetActive(true);
        //UpdateTaskList();
    }
    public override void Return(GameObject player)
    {
        player.GetComponent<Player>().TurnOn();
        //playerTaskPanel.SetActive(false);
        _canInteract = true;
    }

    private void Update()
    {
        if (_canInteract) return;   // no one is on the console

        // unlock left door 
        if (_leftTriggerAction != null && _leftTriggerAction.ReadValue<float>() > 0.1f)
        {
            if (leftDoor != null && leftEscapeDoor != null)
            {
                leftDoor.UnlockDoor();
                leftEscapeDoor.UnlockDoor();
            }
        }
        else 
        {
            if (leftDoor != null && leftEscapeDoor != null)
            {
                leftDoor.LockDoor();
                leftEscapeDoor.LockDoor();
            }
        }

        // unlock right door 
        if (_rightTriggerAction != null && _rightTriggerAction.ReadValue<float>() > 0.1f)
        {
            if (rightDoor != null && rightEscapeDoor != null)
            {
                rightDoor.UnlockDoor();
                rightEscapeDoor.UnlockDoor();
            }
        }
        else
        {
            if (rightDoor != null && rightEscapeDoor != null)
            {
                rightDoor.LockDoor();
                rightEscapeDoor.LockDoor();
            }
        }
    }

    public override bool CanInteract()
    {
        return _canInteract;
    }

    //private void UpdateTaskList()
    //{
    //    // Get all active tasks for the player
    //    var activeTasks = TaskManager.Instance.GetAllActiveTasks();

    //    if (activeTasks.Count == 0)
    //    {
    //        tasksList.text = "No active tasks";
    //        return;
    //    }

    //    string taskText = "";
    //    foreach (var task in activeTasks)
    //    {
    //        taskText += $"{task.title}: {task.currentProgress}/{task.targetProgress}\n";
    //    }

    //    tasksList.text = taskText;
    //}
}
