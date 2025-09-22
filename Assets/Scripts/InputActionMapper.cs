using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Static class to abstract away all the magic strings when retrieving input system InputActions.
/// These methods throw an error if used on an invalid mapping (this would only happen if you are retrieving an action that doesn't belong to the current input actions mapping).
/// </summary>
public static class InputActionMapper
{
    public static InputAction GetCharacterSelectReadyUpAction(PlayerInput playerInput)
    {
        return playerInput.actions.FindAction("ReadyUp", throwIfNotFound: true);
    }
    public static InputAction GetCharacterSelectPlayerLeaveAction(PlayerInput playerInput)
    {
        return playerInput.actions.FindAction("PlayerLeave", throwIfNotFound: true);
    }
    
    public const string CharacterSelectActionMapName = "CharacterSelect";
}
