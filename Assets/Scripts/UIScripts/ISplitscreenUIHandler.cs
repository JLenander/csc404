using UnityEngine;

/// <summary>
/// A UI handler for the Splitscreen player UI
/// </summary>
public interface ISplitscreenUIHandler
{
    /// <summary>
    /// Enable a player's UI overlay
    /// </summary>
    /// <param name="playerIndex">The index of the player to enable</param>
    public void EnablePlayerOverlay(int playerIndex);

    /// <summary>
    /// Disable a player's UI overlay (Player UI overlays are disabled by default)
    /// </summary>
    /// <param name="playerIndex">The index of the player to disable</param>
    public void DisablePlayerOverlay(int playerIndex);

    /// <summary>
    /// Enable (show) and set the color and content of the text that appears when a player hovers over an interactable.
    /// </summary>
    /// <param name="playerIndex">The index of the player to set the interaction text for</param>
    /// <param name="content">The text to set</param>
    /// <param name="msgColour">The color of the text</param>
    public void EnablePlayerInteractionText(int playerIndex, string content, Color msgColour);

    /// <summary>
    /// Disable (hide) the player's interaction text
    /// </summary>
    /// <param name="playerIndex">The player to hide the interaction text for</param>
    public void DisablePlayerInteractionText(int playerIndex);

    /// <summary>
    /// Show the outside camera (or eye camera)
    /// </summary>
    public void ShowOutsideCamera();
    
    /// <summary>
    /// Hide the outside camera (or eye camera)
    /// </summary>
    public void HideOutsideCamera();
}
