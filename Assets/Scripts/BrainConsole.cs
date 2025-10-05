using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class BrainConsole : Interactable
{
    private bool _canInteract = true;
    public GameObject playerTaskPanel;
    public TMPro.TextMeshPro tasksList;
    public GameObject shipControlPanel;

    public override void Interact(GameObject player)
    {
        player.GetComponent<Player>().TurnOff();
        _canInteract = false;

        playerTaskPanel.SetActive(true);
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

    public void TurnOnBrain()   // because i need a reminder to do this
    {

    }
}
