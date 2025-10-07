using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement _minimapRoot;
    private List<VisualElement> _playerDots = new();
    private List<Transform> _playerTransforms = new();

    private Vector2 worldMin = new Vector2(-9, -3);
    private Vector2 worldMax = new Vector2(15, 16);
    // y: 13.3, -1.7
    // x: -8.2, 12.9
    private GlobalPlayerManager _playerManager;
    private PlayerData[] players;
    
    private readonly Color[] _playerColors = {
        Color.red,      // Player 1
        Color.blue,     // Player 2
        Color.yellow,   // Player 3
    };
    
    private bool _initialized = false;
    
    public static MinimapController Instance;
    void Awake() { Instance = this; }

    void Start()
    {
        _minimapRoot = uiDocument.rootVisualElement.Q<VisualElement>("PlayerDotContainer");
        
        _playerManager = FindAnyObjectByType<GlobalPlayerManager>();
        if (_playerManager == null)
        {
            Debug.LogError("No GlobalPlayerManager found!");
            enabled = false;
        }
    }

    void Update()
    {
        if (!_initialized) return;

        UpdatePlayerDots();
    }

    internal void InitializePlayerDots()
    {
        Debug.Log("INIT!");
        players = _playerManager.Players;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Valid)
            {
                _playerTransforms.Add(players[i].PlayerObject.transform);

                // Create a new dot for this player
                var dot = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        width = 20,
                        height = 20,
                        borderBottomLeftRadius = 10,
                        borderBottomRightRadius = 10,
                        borderTopLeftRadius = 10,
                        borderTopRightRadius = 10,
                        backgroundColor = _playerColors[i]
                    }
                };
                _minimapRoot.Add(dot);
                _playerDots.Add(dot);
                
            }
        }
        _initialized = true;
    }

    private void UpdatePlayerDots()
    {
        float mapWidth = _minimapRoot.resolvedStyle.width;
        float mapHeight = _minimapRoot.resolvedStyle.height;

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 worldPos = _playerTransforms[i].position;

            // Map world coordinates to minimap normalized coordinates (0..1)
            float xNorm = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
            float yNorm = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.y);

            // Clamp to 0..1 to avoid dots going off-map
            xNorm = Mathf.Clamp01(xNorm);
            yNorm = Mathf.Clamp01(yNorm);

            float x = xNorm * mapWidth;
            float y = (1 - yNorm) * mapHeight;

            _playerDots[i].style.left = x;
            _playerDots[i].style.top = y;
            
            Debug.Log($"Player {i} world={worldPos} -> map=({x:F1}, {y:F1}) norm=({xNorm:F2},{yNorm:F2})");

        }
    }
    
}
