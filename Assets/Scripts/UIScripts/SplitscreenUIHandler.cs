using UnityEngine;
using UnityEngine.Rendering;
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
    private VisualElement[] _playerInteractionGroups;
    private Label[] _playerInteractionTexts;
    private VisualElement[] _playerGreyscaleOverlays;

    // Camera (outside view or eyes) off overlay
    private VisualElement _outsideCamOverlay;
    private VisualElement _dialogueUI;
    private Label _dialogueText;
    private VisualElement _dialogueIcon;

    private const int NumPlayers = 3;

    void Start()
    {
        DontDestroyOnLoad(this);
        var root = uiDoc.rootVisualElement;

        _player2Overlay = root.Query<VisualElement>("Player2NotJoined").First();
        _player3Overlay = root.Query<VisualElement>("Player3NotJoined").First();

        _playerInteractionGroups = new VisualElement[NumPlayers];
        _playerInteractionTexts = new Label[NumPlayers];
        _playerGreyscaleOverlays = new VisualElement[NumPlayers];
        for (int i = 0; i < NumPlayers; i++)
        {
            _playerInteractionGroups[i] = root.Query<VisualElement>("Player" + (i + 1) + "InteractionGroup").First();
<<<<<<< HEAD
=======
            //_playerInteractionTexts[i] = root.Query<Label>("Player" + (i + 1) + "InteractionText").First();
>>>>>>> d40fb4e (interaction group)
            _playerGreyscaleOverlays[i] = root.Query<VisualElement>("Player" + (i + 1) + "GreyscaleOverlay").First();
            if (_playerGreyscaleOverlays[i] != null)
            {
                _playerGreyscaleOverlays[i].visible = false;
                _playerGreyscaleOverlays[i].SendToBack();
            }
            else
            {
                Debug.Log(_playerGreyscaleOverlays[i] + "no greyscale overlay found");
            }
        }

        _outsideCamOverlay = root.Query<VisualElement>("OutsideCamOffOverlay").First();

        // Dialogue Setup
        _dialogueUI = root.Query<VisualElement>("MessageUI").First();
        _dialogueText = root.Query<Label>("Dialogue").First();
        _dialogueIcon = root.Query<VisualElement>("DialogueIcon").First();
        _dialogueUI.visible = false;

        // Disable Root to start until scene is switched
        root.style.display = DisplayStyle.None;
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    // Handler method to enable or disable Splitscreen UI components based on scene
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // Activate the UI when we enter a scene that is not the Main Menu, Level Select, or Character Select scenes.
        if (SceneConstants.IsCharacterSelectScene() || SceneConstants.IsLevelSelectScene())
        {
            uiDoc.rootVisualElement.style.display = DisplayStyle.None;
        }
        else
        {
            uiDoc.rootVisualElement.style.display = DisplayStyle.Flex;
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
        var group = _playerInteractionGroups[playerIndex];
        if (group == null) return;

        var interactionText = group.Q<Label>("InteractionText");
        interactionText.style.color = msgColour;
        interactionText.text = content;

        group.visible = true;
    }

    public void DisablePlayerInteractionText(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= NumPlayers)
        {
            Debug.LogError("PlayerIndex out of range");
            return;
        }
        _playerInteractionGroups[playerIndex].visible = false;
    }

    public void EnablePlayerScreenGreyscale(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= NumPlayers)
        {
            Debug.LogError("Player index out of range");
            return;
        }

        var overlay = _playerGreyscaleOverlays[playerIndex];
        if (overlay != null)
        {
            overlay.visible = true;
        }
        else
        {
            Debug.LogWarning($"Greyscale overlay for Player {playerIndex + 1} not found.");
        }
    }

    public void DisablePlayerScreenGreyscale(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= NumPlayers)
        {
            Debug.LogError("Player index out of range");
            return;
        }

        var overlay = _playerGreyscaleOverlays[playerIndex];
        if (overlay != null)
        {
            overlay.visible = false;
        }
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

}
