using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectUIHandler : MonoBehaviour, ILevelSelectUIHandler
{
    private VisualElement _root;
    [SerializeField] private VisualTreeAsset levelTemplate;

    // Needs to initialize before Start
    public void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
    }
    
    public void SetupLevelSelectScreen(LevelManager.Level[] levels, Action<int> levelStartHandler)
    {
        var levelsRoot = _root.Query<VisualElement>("Levels").First();

        for (int i = 0; i < levels.Length; i++)
        {
            // Create template element
            VisualElement level = levelTemplate.CloneTree();

            // Copy the int to pass to the event handler
            var levelIndex = i;
            level.AddManipulator(new Clickable(evt => levelStartHandler(levelIndex)));
            
            // Populate fields
            VisualElement levelArtImg = level.Query<VisualElement>("LevelImage").First();
            levelArtImg.style.backgroundImage = new StyleBackground(levels[i].GetLevelArtSprite());
            
            Label levelDisplayName = level.Query<Label>("LevelDisplayName").First();
            levelDisplayName.text = levels[i].displayName;
            
            // Visually indicate level state
            VisualElement lockedOverlay = level.Query<VisualElement>("LockedOverlay").First();
            switch (levels[i].status)
            {
                case  LevelStatus.Unlocked:
                    lockedOverlay.visible = false;
                    break;
                case  LevelStatus.Locked:
                    lockedOverlay.visible = true;
                    break;
                case LevelStatus.Started:
                    // TODO indicate started
                    break;
                case LevelStatus.Completed:
                    // TODO indicate Completed
                    break;
                default:
                    Debug.LogWarning("Unknown level status for level " + levels[i].sceneName);
                    break;
            }
            
            // Add to levels display
            levelsRoot.Add(level);
        }
    }
}

public interface ILevelSelectUIHandler
{
    /// <summary>
    /// Intializes the level select screen which is dynamic based on the levels array
    /// </summary>
    /// <param name="levels">The array of level information from the level manager to set up</param>
    /// <param name="levelStartHandler">The handler for starting a level, passed the index of the level based on the levels array</param>
    public void SetupLevelSelectScreen(LevelManager.Level[] levels, Action<int> levelStartHandler);
}


