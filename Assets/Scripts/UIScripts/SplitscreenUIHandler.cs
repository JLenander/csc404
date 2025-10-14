using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SplitscreenUIHandler : MonoBehaviour, ISplitscreenUIHandler
{
    [SerializeField] private UIDocument uiDoc;

    // Player not joined UI overlays
    // private VisualElement _player1Overlay;
    private VisualElement _player2Overlay;
    private VisualElement _player3Overlay;
    // Player Interaction texts
    private Label[] _playerInteractionTexts;
    // Player Labels and Borders
    private Label[] _playerLabels;
    private VisualElement[] _playerBoxBorders;

    // Camera (outside view or eyes) off overlay
    private VisualElement _outsideCamOverlay;
    private VisualElement _dialogueUI;
    private Label _dialogueText;
    private VisualElement _dialogueIcon;
    private Label _scoreText;

    private const int NumPlayers = 3;

    void Start()
    {
        DontDestroyOnLoad(this);
        var root = uiDoc.rootVisualElement;

        _player2Overlay = root.Query<VisualElement>("Player2NotJoined").First();
        _player3Overlay = root.Query<VisualElement>("Player3NotJoined").First();

        _playerInteractionTexts = new Label[NumPlayers];
        _playerLabels = new Label[NumPlayers];
        _playerBoxBorders = new VisualElement[NumPlayers];
        for (int i = 0; i < NumPlayers; i++)
        {
            _playerInteractionTexts[i] = root.Query<Label>("Player" + (i + 1) + "InteractionText").First();
            _playerLabels[i] = root.Query<Label>("Player" + (i + 1) + "Label").First();
            _playerBoxBorders[i] = root.Query<VisualElement>("Player" + (i + 1));
        }

        _outsideCamOverlay = root.Query<VisualElement>("OutsideCamOffOverlay").First();

        // Dialogue Setup
        _dialogueUI = root.Query<VisualElement>("MessageUI").First();
        _dialogueText = root.Query<Label>("Dialogue").First();
        _dialogueIcon = root.Query<VisualElement>("DialogueIcon").First();
        _dialogueUI.visible = false;

        // score setup
        _scoreText = root.Query<Label>("Score").First();
        _scoreText.visible = false;

        // Disable Root to start until scene is switched
        root.visible = false;
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    // Handler method to enable or disable Splitscreen UI components based on scene
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // Currently behavior is activate whenever the scene changes away from the character select screen but this can be changed
        // in the future to have a whitelist or blacklist
        uiDoc.rootVisualElement.visible = true;
        _scoreText.visible = true;
        
        // Change player box border and label colors based on player colors
        var playerManager = FindAnyObjectByType<GlobalPlayerManager>();
        if (playerManager != null)
        {
            for (int i = 0; i < NumPlayers; i++)
            {
                var playerColor = playerManager.Players[i].PlayerColor;
                _playerBoxBorders[i].style.borderTopColor = playerColor;
                _playerBoxBorders[i].style.borderBottomColor = playerColor;
                _playerBoxBorders[i].style.borderLeftColor = playerColor;
                _playerBoxBorders[i].style.borderRightColor = playerColor;
                _playerLabels[i].style.color = playerColor;
            }
        }
    }

    public void EnablePlayerOverlay(int playerIndex)
    {
        // I used switch statements because we have 3 players
        switch (playerIndex)
        {
            case 0:
                // Currently no player UI
                Debug.LogWarning("Player 1 is enabled by default as we have minimum 1 player");
                break;
            case 1:
                _player2Overlay.visible = false;
                break;
            case 2:
                _player3Overlay.visible = false;
                break;
            default:
                Debug.LogError("PlayerIndex out of range");
                break;
        }
    }

    public void DisablePlayerOverlay(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                // Currently no player UI
                Debug.LogWarning("Player 1 UI cannot be disabled as we have minimum 1 player");
                break;
            case 1:
                _player2Overlay.visible = true;
                break;
            case 2:
                _player3Overlay.visible = true;
                break;
            default:
                Debug.LogError("PlayerIndex out of range");
                break;
        }
    }

    public void EnablePlayerInteractionText(int playerIndex, string content, Color msgColour)
    {
        if (playerIndex < 0 || playerIndex >= NumPlayers)
        {
            Debug.LogError("PlayerIndex out of range");
            return;
        }

        _playerInteractionTexts[playerIndex].style.color = msgColour;
        _playerInteractionTexts[playerIndex].text = content;
        _playerInteractionTexts[playerIndex].visible = true;
    }

    public void DisablePlayerInteractionText(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= NumPlayers)
        {
            Debug.LogError("PlayerIndex out of range");
            return;
        }

        _playerInteractionTexts[playerIndex].visible = false;
    }

    // TODO transition animation
    public void ShowOutsideCamera()
    {
        _outsideCamOverlay.visible = false;
    }

    // TODO transition animation
    public void HideOutsideCamera()
    {
        _outsideCamOverlay.visible = true;
    }

    public void InitializeDialogue()
    {
        _dialogueUI.visible = true;
    }

    public void WriteDialogueText(string content)
    {
        _dialogueText.text = content;
    }

    public void ChangeDialogueSprite(Sprite sprite)
    {
        _dialogueIcon.style.backgroundImage = new StyleBackground(sprite);
    }

    public void HideDialogue()
    {
        _dialogueUI.visible = false;
    }

    public void ChangeScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }

}
