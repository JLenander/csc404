using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// used to control player UI in the splitscreen view, used to shake and dim their cameras
public class GlobalPlayerUIManager : MonoBehaviour
{
    public static GlobalPlayerUIManager Instance;
    private ISplitscreenUIHandler _splitscreenUIHandler;
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
        _splitscreenUIHandler = GameObject.FindAnyObjectByType<SplitscreenUIHandler>();
        DisableDim();
    }

    // log players' cameras
    public void PassPlayers(PlayerData[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Valid)
            {
                playerCam.Add(players[i]);
                _splitscreenUIHandler.EnablePlayerOverlay(i);
            }
        }
        
        start = true;
    }
    
    public void EnableInteractionText(int player, string content, Color msgColour)
    {
        _splitscreenUIHandler.EnablePlayerInteractionText(player, content, msgColour);
    }

    public void DisableInteractionText(int player)
    {
        if (!start) return;
        _splitscreenUIHandler.DisablePlayerInteractionText(player);

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
