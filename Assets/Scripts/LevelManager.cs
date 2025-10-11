using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectUI;
    private ILevelSelectUIHandler _levelSelectUIHandler;
    // For some reason when I serialize this it curses my unity
    private Level[] _levels = {
        new("Level 1", "WalkingCopy", LevelStatus.Unlocked),
        new("Level 2", "WalkingCopy", LevelStatus.Locked),
    };
    
    public void Start()
    {
        _levelSelectUIHandler = levelSelectUI.GetComponent<ILevelSelectUIHandler>();
        InitializeLevelSelectScreen();
    }
    
    private void StartLevel(int levelIndex)
    {
        Debug.Log("Starting Level " + levelIndex);
        if (levelIndex >= _levels.Length)
        {
            Debug.LogError("Level index out of range");
            return;
        }
        
        var level = _levels[levelIndex];
        
        if (level.status != LevelStatus.Locked)
        {
            SceneManager.LoadScene(level.sceneName);
        }
    }

    private void InitializeLevelSelectScreen()
    {
        _levelSelectUIHandler.SetupLevelSelectScreen(_levels, StartLevel);
    }
    
    public struct Level
    {
        public string displayName;
        public string sceneName;
        public LevelStatus status;
        public string levelArtSpriteName;
        // The name of the scene corresponding to the level to unlock after this level is completed.
        public string unlocksScene;

        public Level(string displayName, string sceneName, LevelStatus status, string unlocksScene = "", string levelArtSpriteName = "default_level")
        {
            if (levelArtSpriteName == "")
            {
                levelArtSpriteName = "default_level";
            }
            
            this.displayName = displayName;
            this.sceneName = sceneName;
            this.status = status;
            this.unlocksScene = unlocksScene;
            this.levelArtSpriteName = "LevelSelect/" + levelArtSpriteName;
        }

        public Sprite GetLevelArtSprite()
        {
            // For some reason Resources.Load<Sprite> doesn't work
            var texture = Resources.Load<Texture2D>(levelArtSpriteName);
            var resource = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return resource;
        }
    }
}


public enum LevelStatus
{
    Locked,
    Unlocked,
    Started,
    Completed
}