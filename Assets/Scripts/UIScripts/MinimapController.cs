using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement _minimapRoot;
    private List<VisualElement> _playerDots = new();
    private List<Transform> _playerTransforms = new();

    // TODO: test after new map ui
    private Vector2 worldMin = new Vector2(-6, 1);
    private Vector2 worldMax = new Vector2(12, 19);
    // log: world to map --> x-0.5/1&+1.5/2, y-1.5&+test more
    // x: -8.2, 12.9 -> -9, 15
    // y: -1.7, 13.3 -> -3, 16
    // x: -5.34, 10.34 -> -6, 12
    // y: 2.88, 13.11 -> 1, 19

    private GlobalPlayerManager _playerManager;
    private PlayerData[] players;

    private readonly Color[] _playerColors = {
        Color.red,      // Player 1
        Color.blue,     // Player 2
        Color.yellow,   // Player 3
    };

    private bool _initialized;

    // instance to be used in GlobalPlayerManager to call intialize player dots after players added
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
        // only do actual updating after initialized
        // (Update is being called moment game starts)
        if (!_initialized) return;

        UpdatePlayerDots();
    }

    internal void InitializePlayerDots()
    {
        // get list of PlayerData from GlobalPlayerManager, now that players are added
        players = _playerManager.Players;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].Valid) // note players not joined are still in list just not valid
            {
                // Get player Transform from player object
                // added not recorded so this data will be updated next time checked
                _playerTransforms.Add(players[i].PlayerObject.transform);

                // Create a new dot for this player
                var dot = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        width = 14,
                        height = 14,
                        borderBottomLeftRadius = 7, // half of width/height
                        borderBottomRightRadius = 7,
                        borderTopLeftRadius = 7,
                        borderTopRightRadius = 7,
                        backgroundColor = _playerColors[i]
                    }
                };
                _minimapRoot.Add(dot); // add element to UI
                _playerDots.Add(dot); // add to update
            }
        }
        _initialized = true; // now can update
    }

    private void UpdatePlayerDots()
    {
        // Get minimap dimensions (on UI document)
        float mapWidth = _minimapRoot.resolvedStyle.width;
        float mapHeight = _minimapRoot.resolvedStyle.height;

        for (int i = 0; i < _playerTransforms.Count; i++)
        {
            // Note this is current position because player object's Transform is referenced
            Vector3 worldPos = _playerTransforms[i].position;

            // Map world coordinates to minimap normalized coordinates (0..1)
            float xNorm = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
            float yNorm = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.y);

            // Clamp to 0..1 to avoid dots going off-map
            xNorm = Mathf.Clamp01(xNorm);
            yNorm = Mathf.Clamp01(yNorm);

            // Convert normalized coordinates to minimap pixel coordinates
            float x = xNorm * mapWidth;
            float y = (1 - yNorm) * mapHeight;

            // Update position of dot to reflect in UI
            _playerDots[i].style.left = x;
            _playerDots[i].style.top = y;

            // Debug.Log($"Player {i + 1} World Pos: {worldPos}");
        }
    }

}
