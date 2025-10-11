using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The globall level manager is in charge of the playable levels information, such as the name, the status as to whether
/// it's locked or has been completed, and what level it unlocks next if any.
///
/// Also handles the level select scene.
/// Once a level has been completed, mark it as such with the Complete method (do so before returning to the level select scene).
/// </summary>
public class GlobalLevelManager : MonoBehaviour
{
    public static GlobalLevelManager Instance { get; private set; }
    
    // For some reason when I serialize this it curses my unity
    private readonly Level[] _levels = {
        new("Level 1 - Cafe", "Cafe", LevelStatus.Unlocked, new string[] {"WalkingCopy"}),
        new("Level 2", "WalkingCopy", LevelStatus.Locked),
    };

    private Dictionary<string, int> _sceneNameToLevelIndexMap;

    public void Awake()
    {
        // Only allow one level manager
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else 
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
    }
    
    public void Start()
    {
        _sceneNameToLevelIndexMap = new Dictionary<string, int>();
        for (var i = 0; i < _levels.Length; i++)
        {
            _sceneNameToLevelIndexMap[_levels[i].sceneName] = i;
        }
            
        SanityCheckSceneNames();
    }

    public Level[] GetLevels()
    {
        return _levels;
    }

    /// <summary>
    /// Call to mark a level as complete, unlocking any levels it is guarding.
    /// </summary>
    /// <param name="sceneName">The name of the scene corresponding to the level to mark as complete</param>
    public void CompleteLevel(string sceneName)
    {
        if (!_sceneNameToLevelIndexMap.TryGetValue(sceneName, out var index))
        {
            Debug.LogWarning("Scene " + sceneName + " completed but not in levels array");
            return;
        }

        // Complete this level
        _levels[index].status = LevelStatus.Completed;
        // Unlock next levels if such a relationship exists
        foreach (var unlockedScene in _levels[index].unlocksScenes)
        {
            if (_sceneNameToLevelIndexMap.TryGetValue(unlockedScene, out var unlockedIndex))
            {
                _levels[unlockedIndex].status = LevelStatus.Unlocked;
                Debug.Log("Level " + _levels[index].sceneName + " completion unlocks " + _levels[unlockedIndex].sceneName);
            }
        }
    }

    public void LoadLevelSelectScreen()
    {
        SceneManager.LoadScene(SceneConstants.LevelSelectScene);
    }
    
    public void StartLevel(int levelIndex)
    {
        Debug.Log("Starting Level at index " + levelIndex + " (" + _levels[levelIndex].sceneName + ")");
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
        else
        {
            Debug.Log("Level at index " + levelIndex + " is locked");
        }
    }

    private void SanityCheckSceneNames()
    {
        string[] sceneNames = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var regex = new Regex(@"(?<=/)\w+(?=\.unity)");
            sceneNames[i] = regex.Match(SceneUtility.GetScenePathByBuildIndex(i)).ToString();
        }

        for (int i = 0; i < _levels.Length; i++)
        {
            if (!sceneNames.Contains(_levels[i].sceneName))
            {
                Debug.LogError("Level at index " + i + " (Display Name: \"" + _levels[i].displayName + "\") has scene name \"" + _levels[i].sceneName + "\" but it's not in the scene list.");
                Debug.Log("Scene List: " + string.Join(", ", sceneNames));
            }
        }
    }
    
    public struct Level
    {
        public string displayName;
        public string sceneName;
        public LevelStatus status;
        public string levelArtSpriteName;
        // The name of the scene corresponding to the level to unlock after this level is completed.
        public string[] unlocksScenes;

        public Level(string displayName, string sceneName, LevelStatus status, string[] unlocksScenes = null, string levelArtSpriteName = "default_level")
        {
            if (levelArtSpriteName == "")
            {
                levelArtSpriteName = "default_level";
            }
            
            this.displayName = displayName;
            this.sceneName = sceneName;
            this.status = status;
            if (unlocksScenes == null)
            {
                unlocksScenes = Array.Empty<string>();
            }
            this.unlocksScenes = unlocksScenes;
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