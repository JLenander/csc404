using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UIScripts
{
    /// <summary>
    /// The UI handler for the character selection screen.
    /// </summary>
    public class CharacterSelectHandler : MonoBehaviour, ICharacterSelectScreen
    {
        private const string PlayerCharacterPreviewName = "CharacterPreview";
        
        private VisualElement[] _playerBoxes;
        private VisualElement _readyText;
        
        private int _readyPlayers = 0;
        private int _playerCount = 0;
        
        private readonly Color[] _availableColors = {
            Color.red,      // Player 1
            Color.blue,     // Player 2
            Color.yellow,   // Player 3
        };
        
        private int[] _playerColorIndices = { -1, -1, -1, -1 };
        
        private GlobalPlayerManager _playerManager;
        
        void Start()
        {
            // TODO: no need after new design, just cue scene + spawn anchors
            _playerBoxes = new VisualElement[4];
            
            var root = gameObject.GetComponent<UIDocument>().rootVisualElement;
            _playerBoxes[0] = root.Query<VisualElement>("Player1Selector");
            _playerBoxes[1] = root.Query<VisualElement>("Player2Selector");
            _playerBoxes[2] = root.Query<VisualElement>("Player3Selector");
            _playerBoxes[3] = root.Query<VisualElement>("Player4Selector");

            _readyText = root.Query<VisualElement>("StartGameText");
            
            SetupBoxes();
            
            _playerManager = FindAnyObjectByType<GlobalPlayerManager>();
        }

        // Setup the initial states of the player select boxes
        // TODO: no need after new design
        private void SetupBoxes()
        {
            foreach (var playerBox in _playerBoxes)
            {
                var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
                previewImg.visible = false;
            }
        }
        
        // TODO: spawn players at anchors instead
        public void AddPlayer(int playerIndex)
        {
            // show the character select preview img
            var playerBox =  _playerBoxes[playerIndex];
            var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
            previewImg.visible = true;
            
            // Set player select box background color to player color
            previewImg.style.backgroundColor = _availableColors[playerIndex];
            
            _playerCount++;
        }

        // TODO: despawn players instead
        public void RemovePlayer(int playerIndex)
        {
            // hide character select preview image
            var playerBox =  _playerBoxes[playerIndex];
            var previewImg = playerBox.Query<VisualElement>(name: PlayerCharacterPreviewName).First();
            previewImg.visible = false;
            
            UnreadyPlayer(playerIndex);
            _playerCount--;
        }
        
        // TODO: change order of call -- color check logic in GlobalPlayerManager first, then call here for visual effect
        // TODO: play ready animation instead
        public void ReadyPlayer(int playerIndex)
        {
            // highlight the player box with an outline to indicate they have readied up
            var playerBox =  _playerBoxes[playerIndex];
            playerBox.AddToClassList("playerConfirmed");
            
            _readyPlayers++;
            if (AllPlayersReady())
            {
                _readyText.visible = true;
            }
            else
            {
                _readyText.visible = false;
            }
            
            
        }       
        
        // TODO: play unready animation instead
        public void UnreadyPlayer(int playerIndex)
        {
            // unhighlight the player box
            var playerBox =  _playerBoxes[playerIndex];
            playerBox.RemoveFromClassList("playerConfirmed");
            
            _readyPlayers--;
        }
        
        // True if all players (at least 1) have readied up
        private bool AllPlayersReady()
        {
            return _readyPlayers == _playerCount && _playerCount > 0;
        }
        
        public void ChangeColor(int playerIndex, int direction)
        {
            // Ignore color change if player is ready
            if (_playerManager.Players[playerIndex].Ready)
                return;

            // Cycle index
            var max = _availableColors.Length;
            _playerColorIndices[playerIndex] = (_playerColorIndices[playerIndex] + direction + max) % max;
            var newColor = _availableColors[_playerColorIndices[playerIndex]];

            // Update color box
            var playerBox = _playerBoxes[playerIndex];
            var previewImg = playerBox.Query<VisualElement>("CharacterPreview").First();
            previewImg.style.backgroundColor = newColor;

            // Update GlobalPlayerManager’s color selector
            _playerManager.playerColorSelector[playerIndex] = newColor;
        }

        public void ShowColorTakenWarning(int playerIndex)
        {
            throw new System.NotImplementedException();
        }

        // public void ShowColorTakenWarning(int playerIndex)
        // {
        //     var playerBox = _playerBoxes[playerIndex];
        //     var warning = playerBox.Query<Label>("WarningText");
        //     if (warning != null)
        //     {
        //         warning.text = "Color taken!";
        //         warning.visible = true;
        //         StartCoroutine(HideWarningAfterDelay(warning));
        //     }
        // }
        //
        // private IEnumerator HideWarningAfterDelay(VisualElement warning)
        // {
        //     yield return new WaitForSeconds(1.5f);
        //     warning.visible = false;
        // }


    }
}
