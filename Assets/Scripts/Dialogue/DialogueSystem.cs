using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public SplitscreenUIHandler dialogueUI;
    public float textDelay = 0.05f;   // typing speed
    public AudioSource audioSource;
    public AudioClip clip;

    private Sprite spriteA;              // first image
    private Sprite spriteB;

    private string[] lines;            // dialogue lines

    private int currentLine = 0;
    private bool showingImageA = true;
    private string line;
    private Coroutine dialogueRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartDialogue(DialogueScriptableObj content)
    {
        if (dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine);
            dialogueRoutine = null;
        }

        dialogueUI.InitializeDialogue(); // shows dialogue box
        currentLine = 0;
        lines = content.lines;
        spriteA = content.spirteA;
        spriteB = content.spirteB;
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < lines.Length)
        {
            // Type out the text
            line = "";
            dialogueUI.WriteDialogueText(line);
            foreach (char c in lines[currentLine])
            {
                line += c;
                dialogueUI.WriteDialogueText(line);

                if (c != ' ')
                {
                    showingImageA = !showingImageA;
                    dialogueUI.ChangeDialogueSprite(showingImageA ? spriteA : spriteB);
                    audioSource.PlayOneShot(clip);
                }

                yield return new WaitForSeconds(textDelay);
            }

            // Wait until player presses a key to continue
            yield return new WaitForSeconds(line.Length * 0.05f);

            currentLine++;
        }

        line = "";
        dialogueUI.WriteDialogueText(line);
        dialogueUI.HideDialogue();
    }
}
