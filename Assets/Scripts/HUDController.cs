using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HUDController : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> interactionText;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private List<DialogueSystem> starterDialogue;
    private int index = 0;

    void Start()
    {
        dialogueUI.SetActive(false);
        for (int i = 0; i < interactionText.Count; i++)
        {
            interactionText[i].gameObject.SetActive(false);
        }
    }

    public void EnableInteractionText(int player, string content)
    {
        interactionText[player - 1].text = content;
        interactionText[player - 1].gameObject.SetActive(true);
    }


    public void DisableInteractionText(int player)
    {
        interactionText[player - 1].gameObject.SetActive(false);
    }

    public void playText()
    {
        dialogueUI.SetActive(true);
        starterDialogue[index].StartDialogue();
        index++;
    }
}
