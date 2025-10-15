using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject playerCharacterPrefab;
        
        private readonly Dictionary<int, GameObject> _playerModels = new();
        private readonly Dictionary<int, CharacterSelectController> _playerController = new();
        
        private GlobalPlayerManager _playerManager;
          
        private int _readyPlayers = 0;
        private int _playerCount = 0;
        
        private readonly Color[] _availableColors = {
            Color.red,      // Player 1
            Color.blue,     // Player 2
            Color.yellow,   // Player 3
        };
        
        private int[] _playerColorIndices = { -1, -1, -1 };
        
        // private CharacterSelectController _controller;
        
        [SerializeField] private UIDocument characterSelectUIDocument;
        [SerializeField] private Camera playerSelectCamera;
        
        void Start()
        {
            _playerManager = FindAnyObjectByType<GlobalPlayerManager>();
            if (playerSelectCamera != null)
                playerSelectCamera.gameObject.SetActive(true);   
            if (characterSelectUIDocument != null)
                characterSelectUIDocument.rootVisualElement.visible = false;
        }
        // void OnDestroy()
        // {
        //     if (playerSelectCamera != null)
        //         playerSelectCamera.gameObject.SetActive(false);   
        //     if (characterSelectUIDocument != null)
        //         characterSelectUIDocument.rootVisualElement.visible = false;
        // }
        public void AddPlayer(PlayerInput playerInput, int playerIndex)
        {
            // Spawn the 3D model
            var spawn = spawnPoints[playerIndex];
            var model = Instantiate(playerCharacterPrefab, spawn.position, spawn.rotation);
            model.transform.localScale *= 20f;
            // disable model camera
            var modelCamera = model.GetComponentsInChildren<Camera>();
            foreach (var cam in modelCamera)
                cam.enabled = false;
            // Set the model color
            var outline = model.GetComponent<Outline>();
            outline.OutlineColor = _availableColors[playerIndex];
            // Track the model for despawning later
            _playerModels[playerIndex] = model;

            // Link model to player input
            var _controller = model.GetComponent<CharacterSelectController>();
            _controller.Init(playerInput, playerIndex);
            _playerController[playerIndex] = _controller;
            
            _playerCount++;
        }

        public void RemovePlayer(int playerIndex)
        {
            if (_playerModels.TryGetValue(playerIndex, out var model))
            {
                Destroy(model);
                _playerModels.Remove(playerIndex);
            }             
            
            UnreadyPlayer(playerIndex);
            _playerCount--;
        }
        
        // TODO: play ready animation instead
        public void ReadyPlayer(int playerIndex)
        {
            // TODO: highlight player box to indicate readied up
            // var playerBox =  _playerBoxes[playerIndex];
            // playerBox.AddToClassList("playerConfirmed");
            
            _playerController[playerIndex].ToggleReady(true);
            
            _readyPlayers++;
            
            if (AllPlayersReady())
                Debug.Log("All players ready!");
            
            // TODO: Show "Press __ to start game" text if all players are ready
            // if (AllPlayersReady())
            // {
            //     _readyText.visible = true;
            // }
            // else
            // {
            //     _readyText.visible = false;
            // }
            
        }       
        
        // TODO: play unready animation instead
        public void UnreadyPlayer(int playerIndex)
        {
            // TODO: unhighlight the player box
            // var playerBox =  _playerBoxes[playerIndex];
            // playerBox.RemoveFromClassList("playerConfirmed");
            
            _playerController[playerIndex].ToggleReady(false);
            
            _readyPlayers--;
        }
        
        // True if all players (at least 1) have readied up
        private bool AllPlayersReady()
        {
            return _readyPlayers == _playerCount && _playerCount > 0;
        }
        
        // Called in GLobalPlayerManager when a player changes color (left/right bumper action)
        public void ChangeColor(int playerIndex, int direction)
        {
            // Ignore color change if player is ready
            if (_playerManager.Players[playerIndex].Ready)
                return;

            // Cycle index
            var max = _availableColors.Length;
            _playerColorIndices[playerIndex] = (_playerColorIndices[playerIndex] + direction + max) % max;
            var newColor = _availableColors[_playerColorIndices[playerIndex]];
            _playerController[playerIndex].ChangeColor(newColor);
            
            // Update GlobalPlayerManager’s color selector
            _playerManager.playerColorSelector[playerIndex] = newColor;
            HideColorConflictWarning(playerIndex);
        }

        public void ShowColorConflictWarning(int playerIndex, int otherIndex)
        {
            string message = "Color taken by Player " + otherIndex;
            
            _playerController[playerIndex].ShowLabel(message);
        }

        public void HideColorConflictWarning(int playerIndex)
        {
            _playerController[playerIndex].HideLabel();
        }
        
        // Visualize spawn points in the Scene view
        private void OnDrawGizmos()
        {
            if (spawnPoints == null) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] != null)
                {
                    // Draw a sphere at the spawn point
                    Gizmos.DrawWireSphere(spawnPoints[i].position, 0.25f);

                    // Draw a label with the player number
                    #if UNITY_EDITOR
                    UnityEditor.Handles.Label(spawnPoints[i].position + Vector3.up * 0.3f, "P" + (i + 1));
                    #endif
                }
            }
        }
    }
}
