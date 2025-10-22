using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicSource;
    
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckCurrentScene();
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckCurrentScene();
    }
    
    void CheckCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool shouldPlayMusic = currentScene is "CharacterSelect" or "LevelSelect";
        
        if (shouldPlayMusic && !musicSource.isPlaying)
        {
            musicSource.Play(); // Restarts when coming back from levels
        }
        else if (!shouldPlayMusic && musicSource.isPlaying)
        {
            musicSource.Stop(); // Stops when entering levels
        }
        // If already playing and staying in menus, music continues seamlessly
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}