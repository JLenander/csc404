using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUIHandler : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    
    private VisualElement root;

    private Button _returnToGameButton;
    private Button _returnToLevelSelectButton;
    private Button _quitGameButton;
    
    // Player Sensitivity Settings
    private Slider[] _playerLookSensitivities = new Slider[NumPlayers];
    
    private const int NumPlayers = 3;
    
    private void Awake()
    {
        // DontDestroyOnLoad(this);
        
        root = uiDoc.rootVisualElement;
        
        _returnToGameButton = root.Query<Button>("ReturnToGameButton").First();
        _returnToLevelSelectButton = root.Query<Button>("LevelSelectButton").First();
        _quitGameButton = root.Query<Button>("QuitGameButton").First();
        
        _playerLookSensitivities[0] = root.Query<Slider>("Player1InputSensitivity").First();
        _playerLookSensitivities[1] = root.Query<Slider>("Player2InputSensitivity").First();
        _playerLookSensitivities[2] = root.Query<Slider>("Player3InputSensitivity").First();
        
        HidePauseMenu();
    }

    private void Start()
    {
        // Setup return to level select button callback
        _returnToLevelSelectButton.clicked += () =>
        {
            GlobalLevelManager.Instance.LoadLevelSelectScreen();
        };

        _quitGameButton.clicked += () =>
        {
            Debug.Log("QuitButtonPressed");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(0);
#endif
        };
    }

    public void SetPlayerSettings(int playerIndex, PlayerSettingsUI settings)
    {
        if (playerIndex > NumPlayers)
        {
            Debug.LogError("Player index out of range");
            return;
        } 
        
        _playerLookSensitivities[playerIndex].value = settings.LookSensitivity;
    }

    /// <summary>
    /// Register the callback for a particular player's updated settings. Intended to register one callback per player
    /// that runs when the pause menu is closed.
    /// </summary>
    /// <param name="playerIndex">The index of the player's callback</param>
    /// <param name="callback">A callback function taking the index of the player and the new settings for this player</param>
    public void RegisterPlayerSettingsCallback(int playerIndex, Action<int, PlayerSettingsUI> callback)
    {
        _returnToGameButton.clicked += () =>
        {
            var updatedSettings = new PlayerSettingsUI()
            {
                LookSensitivity = _playerLookSensitivities[playerIndex].value,
            };
            callback(playerIndex, updatedSettings);
        };
    }

    // Enable settings panels for player <playerIndex>
    public void ShowPlayerSettings(int playerIndex)
    {
        if (playerIndex > NumPlayers)
        {
            Debug.LogError("Player index out of range");
            return;
        } 
        
        _playerLookSensitivities[playerIndex].style.display = DisplayStyle.Flex;
    }
    
    // Disable settings panels for player <playerIndex>
    public void HidePlayerSettings(int playerIndex)
    {
        if (playerIndex > NumPlayers)
        {
            Debug.LogError("Player index out of range");
            return;
        } 
        
        _playerLookSensitivities[playerIndex].style.display = DisplayStyle.None;
    }

    public void ShowPauseMenu()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void HidePauseMenu()
    {
        root.style.display = DisplayStyle.None;
    }
    
    // Attempt to put focus on this UI doc (specifically the close pause menu button). Necessary for gamepad focus
    public void FocusPanel()
    {
        _returnToGameButton.Focus();
    }
}

public struct PlayerSettingsUI
{
    // Camera look sensitivity as a plorp
    public float LookSensitivity;
}