using Unity.VisualScripting;
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
    public int maxScore;
    private int evidenceCount;
    private int leftShotCount;
    private int rightShotCount;
    private int leftGrabSuccess;
    private int rightGrabSucess;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this; // easier to reference
        overallScore = 0;
        evidenceCount = 0;
        leftShotCount = 0;
        rightShotCount = 0;
        leftGrabSuccess = 0;
        rightGrabSucess = 0;
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

    public void IncreaseArmCount(bool left)
    {
        if (left)
        {
            leftShotCount++;
        }
        else
        {
            rightShotCount++;
        }
    }

    public void IncreaseGrabCount(bool left)
    {
        if (left)
        {
            leftGrabSuccess++;
        }
        else
        {
            rightGrabSucess++;
        }
    }

    public void LetterScore()
    {
        // give the player a letter grade based on what they got compared
    }


}
