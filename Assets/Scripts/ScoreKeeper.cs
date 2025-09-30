using UnityEngine;

/// <summary>
/// In charge of keeping, adding, and reducing scores
/// </summary>
/// 
/// Players will also be able to view individual stats such as 
/// which arm was used to grab the most, 
/// their arm accuracy, 
/// how many times they’ve launched their arm, 
/// how many evidence objects they have collected, 
/// what penalty actions they triggered, 
/// and whether they succeeded in the level’s special events
public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance;

    private ISplitscreenUIHandler _splitscreenUIHandler;
    private int overallScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this; // easier to reference
        overallScore = 0;
        _splitscreenUIHandler = FindAnyObjectByType<SplitscreenUIHandler>();
        _splitscreenUIHandler.ChangeScoreText(0);
    }

    /// <summary>
    /// Used to add or reduce score
    /// </summary>
    /// <param name="score"></param>
    public void ModifyScore(int score)
    {
        overallScore += score;
        _splitscreenUIHandler.ChangeScoreText(overallScore);
    }
}
