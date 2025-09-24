using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is intended to be on a persistent object handling player input across scenes
/// </summary>
public class GlobalPlayerManager : MonoBehaviour
{
    private int _playerLimit;
    private PlayerData[] _players;
    // The UI handler for the character select screen
    [SerializeField] private GameObject characterSelectScreen;
    private GlobalPlayerUIManager uiManager; // use to aggregate player UI
    private ICharacterSelectScreen _characterSelectScreen;

    void Start()
    {
        DontDestroyOnLoad(this);

        _characterSelectScreen = characterSelectScreen.GetComponent<ICharacterSelectScreen>();

        // initalize player data
        _playerLimit = PlayerInputManager.instance.maxPlayerCount;
        _players = new PlayerData[_playerLimit];
        for (int i = 0; i < _playerLimit; i++)
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
            _players[idx].SubmitActionDelegate = ctx =>
            {
                if (AllPlayersReady())
                {
                    // All players are ready and someone pressed the submit action so we start the game
                    // TODO handle in a level manager?
                    Debug.Log("All players ready - starting");

                    // pass these players to UI manager
                    GlobalPlayerUIManager.Instance.PassPlayers(_players);

                    SceneManager.LoadScene("Tutorial");
                }
                else
                {
                    Debug.Log("Player " + idx + " ready");
                    _characterSelectScreen.ReadyPlayer(idx);
                    _players[idx].Ready = true;
                }
                Debug.Log("submit action");
            };
            _players[idx].CancelActionDelegate = ctx =>
            {
                // Unready a player or remove them if they're already unready.
                if (_players[idx].Ready)
                {
                    Debug.Log("Player " + idx + " not ready");
                    _characterSelectScreen.UnreadyPlayer(idx);
                    _players[idx].Ready = false;
                }
                else
                {
                    Debug.Log("Player " + idx + " leaving");
                    _characterSelectScreen.RemovePlayer(idx);
                    Destroy(playerInput.gameObject);
                }
            };
            InputActionMapper.GetCharacterSelectSubmitAction(playerInput).started += _players[idx].SubmitActionDelegate;
            InputActionMapper.GetCharacterSelectCancelAction(playerInput).started += _players[idx].CancelActionDelegate;

            // Ensure player is on the character select screen action map
            playerInput.SwitchCurrentActionMap(InputActionMapper.CharacterSelectActionMapName);
            playerInput.gameObject.GetComponent<Player>().TurnOff();
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
            InputActionMapper.GetCharacterSelectSubmitAction(playerInput).started -= _players[playerInput.playerIndex].SubmitActionDelegate;
            InputActionMapper.GetCharacterSelectCancelAction(playerInput).started -= _players[playerInput.playerIndex].CancelActionDelegate;
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
                // TODO handle this in the level manager? At least make it more efficient.
                var spawnAnchor = GameObject.Find("Player" + (player.Index + 1) + "Spawn");

                // Teleport player to their spawn anchor for this new scene
                var charController = player.PlayerObject.GetComponent<CharacterController>();
                charController.enabled = false;
                Debug.Log("Attempting scene change player " + player.Index + " teleport to anchor for new scene " + newScene.name);
                player.PlayerObject.transform.position = spawnAnchor.transform.position;
                charController.enabled = true;

                // Switch action map to player action map if not character selection screen
                if (IsCharacterSelectScene())
                {
                    player.Input.SwitchCurrentActionMap(InputActionMapper.CharacterSelectActionMapName);
                }
                else
                {
                    player.Input.SwitchCurrentActionMap(InputActionMapper.PlayerActionMapName);
                }

                // re-enable player
                player.PlayerObject.GetComponent<Player>().TurnOn();
            }
        }

        // disable joining if not in the character select scene
        if (!IsCharacterSelectScene())
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }

    /// <returns>True iff all valid players are ready and at least one player is valid</returns>
    private bool AllPlayersReady()
    {
        return _players.All(player => !player.Valid || player.Ready) && _players.Any(player => player.Valid);
    }

    /// <returns>True if this is the character select scene. False otherwise</returns>
    private static bool IsCharacterSelectScene()
    {
        return SceneManager.GetActiveScene().name == "CharacterSelect";
    }

}

public struct PlayerData
{
    // True if the player is a valid playerdata object with an active player input
    public bool Valid { get; set; }
    // True if the player is ready to start the game (used in the character select screen)
    public bool Ready { get; set; }
    public int Index { get; set; }
    public PlayerInput Input { get; set; }
    public GameObject PlayerObject { get; set; }
    public Action<InputAction.CallbackContext> SubmitActionDelegate { get; set; }
    public Action<InputAction.CallbackContext> CancelActionDelegate { get; set; }
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

    /// <summary>
    /// A player has unreadied and can interact with the character selection again.
    /// </summary>
    /// <param name="playerIndex"></param>
    public void UnreadyPlayer(int playerIndex);
}