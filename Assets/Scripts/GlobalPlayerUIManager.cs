using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// used to aggregate player UI, used to shake and dim their cameras
public class GlobalPlayerUIManager : MonoBehaviour
{
    public static GlobalPlayerUIManager Instance;
    [SerializeField] private TMP_Text textPrefab; // prefab of prompt text
    [SerializeField] private List<TMP_Text> interactionText = new List<TMP_Text>(); // player texts
    [SerializeField] private GameObject dialogueUI; // dialogue visua
    [SerializeField] private List<DialogueSystem> starterDialogue = new List<DialogueSystem>(); // dialogues
    [SerializeField] private RectTransform canvas; // main canvas
    [SerializeField] private Image cameraDim; // image used to dim camera
    [SerializeField] private float dim = 0.796f; // dim amount, range 0 to 1
    private List<PlayerData> playerCam = new List<PlayerData>();

    private bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; // easier to reference
        DisableDim();
    }

    // log players' cameras
    public void PassPlayers(PlayerData[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Valid)
                playerCam.Add(players[i]);
        }

        ArrangeText();

        start = true;
    }

    // depending on number of players we set up UI text
    void ArrangeText()
    {
        // clear real quick in case left from last scene
        foreach (var t in interactionText)
            Destroy(t.gameObject);
        interactionText.Clear();

        int count = playerCam.Count;
        Debug.Log("Players: " + count);

        // depending on number of players we set up UI text
        for (int i = 0; i < count; i++)
        {
            // initialize text from prefab
            interactionText.Add(Instantiate(textPrefab, canvas));
            RectTransform textTransform = interactionText[i].rectTransform;

            if (count == 1)
            {
                // anchor set at centre of screen
                textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.5f, 0.5f);
                textTransform.anchoredPosition = new Vector2(0, -139f); // lower a bit
            }
            else if (count == 2)
            {
                // left then right leaning anchor
                if (i == 0)
                    textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.25f, 0.5f);
                else
                    textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.75f, 0.5f);

                textTransform.anchoredPosition = new Vector2(0, -139f); // lower a bit
            }
            else
            {
                // each corner
                switch (i)
                {
                    case 0: textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.25f, 0.75f); break; // top-left
                    case 1: textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.75f, 0.75f); break; // top-right
                    case 2: textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.25f, 0.25f); break; // bottom-left
                    case 3: textTransform.anchorMin = textTransform.anchorMax = new Vector2(0.75f, 0.25f); break; // bottom-right
                }

                textTransform.anchoredPosition = new Vector2(0, -44f); // lower even less
            }

            interactionText[i].gameObject.SetActive(false); // hide text for now
        }
    }

    public void EnableInteractionText(int player, string content, Color msgColour)
    {
        interactionText[player].text = content;
        interactionText[player].color = msgColour;
        interactionText[player].gameObject.SetActive(true);
    }

    public void DisableInteractionText(int player)
    {
        if (!start) return;
        interactionText[player].gameObject.SetActive(false);
    }

    // fades image into view based on *time* seconds, used for blink terminal
    public void FadeView(float time)
    {
        // StartCoroutine(FadeRoutine(time));
        Debug.Log("Start telling the player");
    }

    IEnumerator FadeRoutine(float time)
    {
        if (cameraDim == null)
            yield break; // missing image

        Color colour = cameraDim.color;
        float startAlpha = 0f;
        float targetAlpha = dim;

        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);

            colour.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            Debug.Log(colour.a);
            cameraDim.color = colour;

            yield return null;
        }
    }

    public void DisableDim()
    {
        // alpha to 0
        Color colour = cameraDim.color;
        colour.a = 0;
        cameraDim.color = colour;
    }
}
