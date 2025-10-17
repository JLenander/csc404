using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OverlayUIHandler : MonoBehaviour
{
    public UIDocument uIDocument;
    public string containerName;
    protected VisualElement root;
    protected VisualElement container;
    private GameObject _currPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        root = uIDocument.rootVisualElement;

        container = root.Query<VisualElement>(containerName).First();

        if (container == null)
        {
            Debug.Log("No container name set for " + this);
        }

        HideContainer(null);
    }

    public virtual void HideContainer(GameObject player)
    {
        // hide container
        if (container != null)
            container.style.display = DisplayStyle.None;
        // unlock player
        if (player != null)
            player.GetComponent<Player>().TurnOn();
        _currPlayer = null;
    }

    public virtual void ShowContainer(GameObject player)
    {
        // show container
        if (container != null)
            container.style.display = DisplayStyle.Flex;
        // disable player movement
        player.GetComponent<Player>().TurnOff();
        _currPlayer = player;

        // move to player's screen
        int playerId = player.GetComponent<PlayerInput>().playerIndex;
        
        MoveToPlayer(playerId);
    }

    void MoveToPlayer(int playerId)
    {
        float leftPercent = 0f;
        float topPercent = 0f;

        switch (playerId)
        {
            case 0: // top-left
                leftPercent = 0f;
                topPercent = 0f;
                break;
            case 1: // top-right
                leftPercent = 50f;
                topPercent = 0f;
                break;
            case 2: // bot-left
                leftPercent = 0f;
                topPercent = 50f;
                break;
        }

        if (container != null)
        {
            container.style.left = new Length(leftPercent, LengthUnit.Percent);
            container.style.top = new Length(topPercent, LengthUnit.Percent);
        }

    }

}
