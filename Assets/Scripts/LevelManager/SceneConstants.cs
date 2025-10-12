using UnityEngine.SceneManagement;

public class SceneConstants
{
    public const string MainMenuScene = "Menu";
    public const string CharacterSelectScene = "CharacterSelect";
    public const string LevelSelectScene = "LevelSelect";
    
    /// <returns>True if this is the character select scene. False otherwise</returns>
    public static bool IsCharacterSelectScene()
    {
        return SceneManager.GetActiveScene().name == CharacterSelectScene;
    } 
    
    /// <returns>True if this is the Level Select scene. False otherwise</returns>
    public static bool IsLevelSelectScene()
    {
        return SceneManager.GetActiveScene().name == LevelSelectScene;
    }
}
