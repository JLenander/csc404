using System.Collections;
using UnityEngine;

public class FillCup : MonoBehaviour
{
    [SerializeField] private Transform liquid;
    [SerializeField] private int fullCounter;
    [SerializeField] private float maxFillHeight = 1f;
    [SerializeField] private float initialHeight = 0.1f;

    private bool full = false;

    private int counter;
    private Vector3 baseScale;

    public Outline outline;

    void Start()
    {
        float newYScale = maxFillHeight;
        float yOffset = (newYScale - initialHeight) / 1.5f;
        baseScale = new Vector3(3.5f, initialHeight, 3.5f);
        liquid.localScale = baseScale;
        liquid.localPosition = new Vector3(0f, yOffset, 0f);
        StartCoroutine(WaitForScoreKeeper());
        DisableOutline();
    }

    IEnumerator WaitForScoreKeeper()
    {
        yield return new WaitUntil(() => ScoreKeeper.Instance != null);
        ScoreKeeper.Instance.AddScoring("Filled Nova's coffee", 5, false, true, 0);
    }

    public void StartTask()
    {
        // trigger nova animation (maybe done outside)

        // set coffee to empty

        baseScale = new Vector3(3.5f, initialHeight, 3.5f);
        liquid.localScale = baseScale;
        liquid.localPosition = Vector3.zero;
        counter = 0;

        // allow filling
        full = false;
    }

    public void AddCoffee()
    {
        if (full) return;

        EnableOutline();

        counter++;
        if (counter > fullCounter)
        {
            full = true;
            ScoreKeeper.Instance.IncrementScoring("Filled Nova's coffee");
            Level1TaskManager.CompleteTaskPourCoffee();
        }

        float fillProgress = (float)counter / fullCounter;
        float newYScale = Mathf.Lerp(initialHeight, maxFillHeight, fillProgress);
        float yOffset = (newYScale - initialHeight) / 1.5f;

        liquid.localScale = new Vector3(baseScale.x, newYScale, baseScale.z);
        liquid.localPosition = new Vector3(0f, yOffset, 0f);
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}
