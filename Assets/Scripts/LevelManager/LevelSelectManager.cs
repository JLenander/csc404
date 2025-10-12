using UnityEngine;

/// <summary>
/// This exists to connect the level select UI to the current global level manager's data source.
/// </summary>
public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectUI;
    private ILevelSelectUIHandler _levelSelectUIHandler;

    public void Start()
    {
        _levelSelectUIHandler = levelSelectUI.GetComponent<ILevelSelectUIHandler>();
        _levelSelectUIHandler.SetupLevelSelectScreen(GlobalLevelManager.Instance.GetLevels(), GlobalLevelManager.Instance.StartLevel);
    }
}
