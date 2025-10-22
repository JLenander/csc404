using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ConsoleUIHandler : OverlayUIHandler
{
    public override void ShowContainer(GameObject player)
    {
        base.ShowContainer(player);
        ApplyPlayerColor(player.GetComponent<PlayerInput>().playerIndex);
    }

    void ApplyPlayerColor(int playerId)
    {
        Color playerColor = GlobalPlayerManager.Instance.Players[playerId].PlayerColor;

        var borders = root.Query<VisualElement>(className: "instructions-border").ToList();
        var headers = root.Query<VisualElement>(className: "instructions-header").ToList();
        var texts = root.Query<Label>(className: "instructions-text").ToList();

        // Update their styles
        foreach (var b in borders)
        {
            b.style.borderLeftColor = playerColor;
            b.style.borderRightColor = playerColor;
            b.style.borderTopColor = playerColor;
            b.style.borderBottomColor = playerColor;
        }

        foreach (var h in headers)
            h.style.backgroundColor = playerColor;

        foreach (var t in texts)
            t.style.color = playerColor;
    }
}
