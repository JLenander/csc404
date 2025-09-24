using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

// used to aggregate player UI, used to shake and dim their cameras
public class GlobalPlayerUIManager : MonoBehaviour
{
    public static GlobalPlayerUIManager Instance;
    [SerializeField] private TMP_Text textPrefab;
    [SerializeField] private List<TMP_Text> interactionText = new List<TMP_Text>();
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private List<DialogueSystem> starterDialogue = new List<DialogueSystem>();
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Image cameraDim;
    private List<PlayerData> playerCam = new List<PlayerData>();

    private bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; // easier to reference
    }

    // Update is called once per frame
    void Update()
    {

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

    public void EnableInteractionText(int player, string content)
    {
        interactionText[player].text = content;
        interactionText[player].gameObject.SetActive(true);
    }


    public void DisableInteractionText(int player)
    {
        if (!start) return;
        interactionText[player].gameObject.SetActive(false);
    }
}
