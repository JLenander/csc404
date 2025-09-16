using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueUI;
    public TMP_Text dialogueText;     // assign in Inspector
    public Image spriteImage;
    public Sprite spriteA;              // first image
    public Sprite spriteB;

    public string[] lines;            // dialogue lines
    public float textDelay = 0.05f;   // typing speed

    public AudioSource audioSource;
    public AudioClip clip;

    private int currentLine = 0;
    private bool showingImageA = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartDialogue()
    {
        currentLine = 0;
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < lines.Length)
        {
            // Type out the text
            dialogueText.text = "";
            foreach (char c in lines[currentLine])
            {
                dialogueText.text += c;

                if (c != ' ')
                {
                    showingImageA = !showingImageA;
                    spriteImage.sprite = showingImageA ? spriteA : spriteB;
                    audioSource.PlayOneShot(clip);
                }

                yield return new WaitForSeconds(textDelay);
            }

            // Wait until player presses a key to continue
            yield return new WaitForSeconds(dialogueText.text.Length * 0.05f);

            currentLine++;
        }

        dialogueText.text = "";
        dialogueUI.SetActive(false);
    }
}
