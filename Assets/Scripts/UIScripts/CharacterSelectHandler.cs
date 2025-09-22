using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UIScripts
{
public class CharacterSelectHandler : MonoBehaviour, ICharacterSelectScreen
{
    private const string PlayerCharacterPreviewName = "CharacterPreview";
    
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
    }

    // Setup the initial states of the player select boxes
    private void SetupBoxes()
    {
        foreach (var playerBox in _playerBoxes)
        {
            var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
            previewImg.visible = false;
        }
    }
    
    public void AddPlayer(int playerIndex)
    {
        // show the character select preview img
        var playerBox =  _playerBoxes[playerIndex];
        var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
        previewImg.visible = true;
    }

    public void RemovePlayer(int playerIndex)
    {
        // hide character select preview image
        var playerBox =  _playerBoxes[playerIndex];
        var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
        previewImg.visible = false;
        // unhighlight the player box
        playerBox.RemoveFromClassList("playerConfirmed");

    }
    
    public void ReadyPlayer(int playerIndex)
    {
        // highlight the player box with an outline to indicate they have readied up
        var playerBox =  _playerBoxes[playerIndex];
        playerBox.AddToClassList("playerConfirmed");
    }
}
}
