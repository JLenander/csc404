using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is intended to be on a persistent object handling player input across scenes
/// </summary>
public class GlobalPlayerManager : MonoBehaviour
{
    [SerializeField] private int playerLimit;
    private PlayerData[] _players;
    // The UI handler for the character select screen
    [SerializeField] private GameObject characterSelectScreen;
    private ICharacterSelectScreen _characterSelectScreen;
    
    void Start()
    {
        DontDestroyOnLoad(this);
        
        _characterSelectScreen = characterSelectScreen.GetComponent<ICharacterSelectScreen>();
        
        // initalize player data
        _players = new PlayerData[playerLimit];
        for (int i = 0; i < playerLimit; i++)
        {
            _players[i].Index = i;
        }
        
        // Register handlers for when a player joins or leaves
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        
        // Register handler for when the scene changes
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    /// <summary>
    /// Handler method for when a player joins.
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (IsCharacterSelectScene())
        {
            var idx = playerInput.playerIndex;
            Debug.Log("Player " + idx + " Joined - Character Select Scene");
            _players[idx].Input = playerInput;
            _players[idx].PlayerObject = playerInput.gameObject; // This might change so it's a separate field.
            _players[idx].Valid = true;
            
            // Add player to the character selection screen so they can start selecting their character.
            _characterSelectScreen.AddPlayer(idx);
            
            // register callbacks for the character select screen actions.
            _players[idx].ReadyPlayerDelegate = ctx =>
            {
                Debug.Log("Player " + idx+ " ready");
                _characterSelectScreen.ReadyPlayer(playerInput.playerIndex);
            };
            _players[idx].PlayerLeaveDelegate = ctx =>
            {
                Debug.Log("Player " +idx + " leaving");
                _characterSelectScreen.RemovePlayer(playerInput.playerIndex);
                Destroy(playerInput.gameObject);
            };
            InputActionMapper.GetCharacterSelectReadyUpAction(playerInput).started += _players[idx].ReadyPlayerDelegate;
            InputActionMapper.GetCharacterSelectPlayerLeaveAction(playerInput).started += _players[idx].PlayerLeaveDelegate;
            
            // Ensure player is on the character select screen action map
            playerInput.SwitchCurrentActionMap(InputActionMapper.CharacterSelectActionMapName);
        }
        else
        {
            Debug.LogWarning("Player attempted to join - Other Scene");
        }
    }
    
    /// <summary>
    /// Handler method for when a player leaves
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (IsCharacterSelectScene())
        {
            Debug.Log("Player " + playerInput.playerIndex + " Left - Character Select Scene");
            
            // Remove the registered callbacks
            InputActionMapper.GetCharacterSelectReadyUpAction(playerInput).started -= _players[playerInput.playerIndex].ReadyPlayerDelegate;
            InputActionMapper.GetCharacterSelectPlayerLeaveAction(playerInput).started -= _players[playerInput.playerIndex].PlayerLeaveDelegate;
        }
        else
        {
            Debug.Log("Player Left - Other Scene");
        }
    }

    /// <summary>
    /// Handler for managing players when the scene changes
    /// </summary>
    /// <param name="oldScene"></param>
    /// <param name="newScene"></param>
    private void ActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        foreach (var player in _players)
        {
            if (player.Valid)
            {
                // Find player's spawn anchor for this scene
                
                // Teleport player to their spawn anchor for this new scene
                var charController = player.PlayerObject.GetComponent<CharacterController>();
                charController.enabled = false;
                Debug.Log("Attempting scene change player teleport to anchor for new scene " + newScene.name);
                // player.PlayerObject.transform = 
                charController.enabled = true;
            }
        }
    }
    
    /// <returns>True if this is the character select scene. False otherwise</returns>
    private static bool IsCharacterSelectScene()
    {
        return SceneManager.GetActiveScene().name == "CharacterSelect";
    }
    
}

public struct PlayerData
{
    public bool Valid { get; set; }
    public int Index { get; set; }
    public PlayerInput Input { get; set; }
    public GameObject PlayerObject { get; set; }
    public Action<InputAction.CallbackContext> ReadyPlayerDelegate { get; set; }
    public Action<InputAction.CallbackContext> PlayerLeaveDelegate { get; set; }
}

public interface ICharacterSelectScreen
{
    /// <summary>
    /// Add a player to the character selection screen to allow them to select their character.
    /// </summary>
    /// <param name="playerIndex"></param>
    public void AddPlayer(int playerIndex);
    
    /// <summary>
    /// Remove a player by index from the character selection screen.
    /// </summary>
    /// <param name="playerIndex"></param>
    public void RemovePlayer(int playerIndex);
    
    /// <summary>
    /// A player has readied up and has confirmed their selection.
    /// </summary>
    /// <param name="playerIndex">The index of the player who readied up</param>
    public void ReadyPlayer(int playerIndex);
}
//
// private Enum {
//
// }