using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreboardUIHandler : MonoBehaviour
{
    public float typeDelay = 0.05f; // delay per character
    public float pauseAfterTitle = 0.5f;
    public float scoreCountDuration = 1f; // how fast number goes up, perhaps scale with amount ltr
    public float betweenTitles = 0.3f;

    public string evidenceCountText = "Number of evidence collected";
    public string dominanteLeftText = "Hand that was used the most ";
    public string hurtDateCountText = "Number of times you've hit your date";

    [SerializeField] private UIDocument scoreboardDoc;
    private VisualElement scoreboardContainer;
    private Label scoreboardContent;
    private Label letterGrade;
    private Label letterGradeTitle;

    private int textWidth = 55;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = scoreboardDoc.rootVisualElement;
        // find all needed UI elements
        scoreboardContent = root.Query<Label>("ScoreboardContent").First();
        scoreboardContainer = root.Query<VisualElement>("ScoreboardContainer").First();
        letterGrade = root.Query<Label>("LetterGrade").First();
        letterGradeTitle = root.Query<Label>("LetterGradeTitle").First();

        // disable for now
        letterGradeTitle.visible = false;
        letterGrade.visible = false;
        scoreboardContent.visible = false;
        scoreboardContainer.visible = false;
    }

    // given a event and its count, dispaly on UI and also increment count
    void ShowScoreboard()
    {
        scoreboardContent.visible = true;
        scoreboardContainer.visible = true;
        // get scores from scorekeeper
        ScoreboardData data = ScoreKeeper.Instance.GetScores();

        // // test
        // ScoreboardData data = new ScoreboardData();

        // data.evidenceCount = 100;
        // data.dominanteLeft = false;
        // data.hurtDateCount = 2;
        // data.letter = "G";

        // clear content
        scoreboardContent.text = "";

        StartCoroutine(AnimateScoreboardRoutine(data));
    }

    string AddDashes(string title)
    {
        int dashCount = Mathf.Max(0, textWidth - title.Length);
        string dashes = new string('-', dashCount);
        return title + " " + dashes + " x";
    }

    IEnumerator AnimateScoreboardRoutine(ScoreboardData data)
    {
        scoreboardContent.visible = true;
        // print reoccuring data first
        string content = AddDashes(evidenceCountText);
        scoreboardContent.text = content;

        // pause a bit
        yield return new WaitForSeconds(pauseAfterTitle);

        // increment number
        float elapsed = 0f;
        int startScore = 0;
        int targetScore = data.evidenceCount;

        while (elapsed < scoreCountDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scoreCountDuration);
            int current = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreboardContent.text = content + current.ToString();
            yield return null;
        }

        content = content + targetScore + "\n";
        scoreboardContent.text = content;

        yield return new WaitForSeconds(betweenTitles); // small pause

        content = content + AddDashes(hurtDateCountText);
        scoreboardContent.text = content;

        // pause a bit
        yield return new WaitForSeconds(pauseAfterTitle);

        // increment number
        elapsed = 0f;
        startScore = 0;
        targetScore = data.hurtDateCount;

        while (elapsed < scoreCountDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scoreCountDuration);
            int current = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreboardContent.text = content + current.ToString();
            yield return null;
        }

        content = content + targetScore + "\n";
        scoreboardContent.text = content;

        // pause a bit
        yield return new WaitForSeconds(pauseAfterTitle);

        // event stuff

        // Dominant hand
        content = content + dominanteLeftText;
        if (data.dominanteLeft)
        {
            string dashes = new string('-', textWidth - dominanteLeftText.Length - 4);
            content = content + dashes + " Left\n";
        }
        else
        {
            string dashes = new string('-', textWidth - dominanteLeftText.Length - 6);
            content = content + dashes + " Right\n";
        }
        scoreboardContent.text = content;

        yield return new WaitForSeconds(betweenTitles); // small pause


        // letter grade
        letterGradeTitle.visible = true;

        yield return new WaitForSeconds(2f); // small pause

        letterGrade.text = data.letter;
        letterGrade.visible = true;
    }

    // for reference
    // public struct ScoreboardData
    // {
    //     public int evidenceCount;
    //     public bool dominanteLeft;
    //     public int hurtDateCount;
    //     public string letter;
    //     public List<Scoring> events;
    // }
}
