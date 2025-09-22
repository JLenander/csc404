using UnityEngine;
using UnityEngine.UIElements;

namespace UIScripts
{
public class CharacterSelectHandler : MonoBehaviour
{
    private VisualElement[] _playerBoxes;
    void Start()
    {
        _playerBoxes = new VisualElement[4];
        
        var root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        _playerBoxes[0] = root.Query<VisualElement>("Player1Selector");
        _playerBoxes[1] = root.Query<VisualElement>("Player2Selector");
        _playerBoxes[2] = root.Query<VisualElement>("Player3Selector");
        _playerBoxes[3] = root.Query<VisualElement>("Player4Selector");
        
        SetupBoxes();
        ConfirmPlayer(0);
    }

    // Setup the intial states of the player select boxes
    private void SetupBoxes()
    {
        foreach (var playerBox in _playerBoxes)
        {
            var previewImg = playerBox.Query<VisualElement>(name: "CharacterPreview").First();
            previewImg.visible = false;
        }
    }

    // Modifies the player select box for the player specified  by <playerIndex> to visualize they have confirmed their charaacter selection
    public void ConfirmPlayer(int playerIndex)
    {
        var playerBox =  _playerBoxes[playerIndex];
        var previewImg = playerBox.Query<VisualElement>(name: "CharacterPreview").First();
        previewImg.visible = true;
        playerBox.AddToClassList("playerConfirmed");
    }
}
}
