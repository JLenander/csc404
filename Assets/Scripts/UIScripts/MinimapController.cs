using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement _minimapRoot;
    private List<Transform> _playerTransforms = new();
    private List<VisualElement> _playerWrappers = new();
    
    // size of wrapper square in pixels (tweakable)
    private const int WrapperSize = 32; // around 2x dot size for good spacing
    private const int DotSize = 14; // size of player dot in pixels (tweakable), also used for cone width

    // TODO: test after new map ui
    private readonly Vector2 _worldMin = new Vector2(-6, 2);
    private readonly Vector2 _worldMax = new Vector2(11, 19);
    // log: world to map --> x-0.5/1&+1/1.5, y-1.5&+test more
    // x: -8.2, 12.9 -> -9, 15
    // y: -1.7, 13.3 -> -3, 16
    // x: -5.34, 10.34 -> -6, 11
    // y: 2.88, 13.11 -> 2, 19

    private GlobalPlayerManager _playerManager;
    private PlayerData[] _players;

    private bool _initialized;

    // instance to be used in GlobalPlayerManager to call initialize player dots after players added
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
        _players = _playerManager.Players;

        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i].Valid) // note players not joined are still in list just not valid
            {
                // Get player Transform from player object
                // added not recorded so this data will be updated next time checked
                _playerTransforms.Add(_players[i].PlayerObject.transform);
                
                // --- Wrapper (rotates around its center) ---
                var wrapper = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        width = WrapperSize,
                        height = WrapperSize,
                        // Set transform origin to center for proper rotation
                        // Must Length.Percent(50) not just 50
                        transformOrigin = new StyleTransformOrigin(new TransformOrigin(Length.Percent(50), Length.Percent(50)))
                    }
                };

                // --- Cone (inside wrapper) ---
                // basically a triangle made using borders
                var coneImage = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        // note width height are for the bounding box, not the triangle itself
                        // only bigger no smaller than border widths
                        width = DotSize,
                        height = DotSize + 3, // since borderTopWidth is DotSize + 3
                        left = (WrapperSize - DotSize) / 2f, // Center horizontally
                        bottom = -4, // make position out from dot (higher the further)
                        // essentially "unset" top so can point other way (outward from dot)
                        top = StyleKeyword.Auto, 
                        // Use borders to create triangle pointing DOWNWARD (outward)
                        borderLeftWidth = DotSize / 2,
                        borderRightWidth = DotSize / 2,
                        borderTopWidth = DotSize + 3,    // Height of triangle
                        borderBottomWidth = 0,
                        borderLeftColor = Color.clear,
                        borderRightColor = Color.clear,
                        borderTopColor = _players[i].PlayerColor,
                        backgroundColor = Color.clear
                    }
                };

                // --- Player dot centered within wrapper ---
                var dot = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        width = DotSize,
                        height = DotSize,
                        left = (WrapperSize / 2f) - (DotSize / 2f),
                        top = (WrapperSize / 2f) - (DotSize / 2f),
                        borderBottomLeftRadius = Length.Percent(50),
                        borderBottomRightRadius = Length.Percent(50),
                        borderTopLeftRadius = Length.Percent(50),
                        borderTopRightRadius = Length.Percent(50),
                        backgroundColor = _players[i].PlayerColor
                    }
                };

                // layer cone below dot
                wrapper.Add(coneImage);
                wrapper.Add(dot);

                _minimapRoot.Add(wrapper);
                _playerWrappers.Add(wrapper);
                
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
            float xNorm = Mathf.InverseLerp(_worldMin.x, _worldMax.x, worldPos.x);
            float yNorm = Mathf.InverseLerp(_worldMin.y, _worldMax.y, worldPos.y);

            // Clamp to 0..1 to avoid dots going off-map
            xNorm = Mathf.Clamp01(xNorm);
            yNorm = Mathf.Clamp01(yNorm);

            // Convert normalized coordinates to minimap pixel coordinates
            float x = xNorm * mapWidth;
            float y = (1 - yNorm) * mapHeight;
            
            // Position wrapper so its center is at x,y
            float wrapperLeft = x - (WrapperSize / 2f);
            float wrapperTop  = y - (WrapperSize / 2f);

            _playerWrappers[i].style.left = wrapperLeft;
            _playerWrappers[i].style.top = wrapperTop;
            
            float yaw = _playerTransforms[i].eulerAngles.y;
            // add 180 so facing neg y is upwards on minimap
            float minimapYaw = (yaw + 180f) % 360f;

            // Apply rotation to whole wrapper
            _playerWrappers[i].style.rotate = new Rotate(minimapYaw);
        
            // For testing world limits
            // Debug.Log($"Player {i + 1} World Pos: {worldPos}");
        }
    }

}
