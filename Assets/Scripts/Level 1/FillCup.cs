using System.Collections;
using UnityEngine;
using UnityEngineInternal;

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
        baseScale = new Vector3(3.5f, initialHeight, 3.5f);
        liquid.localScale = baseScale;
        liquid.localPosition = Vector3.zero;
        counter = 0;
        StartCoroutine(WaitForScoreKeeper());
        DisableOutline();
    }

    IEnumerator WaitForScoreKeeper()
    {
        yield return new WaitUntil(() => ScoreKeeper.Instance != null);
        ScoreKeeper.Instance.AddScoring("Filled Nova's coffee", 5, false, true, 0);
    }

    public void AddCoffee()
    {   
        EnableOutline();
        if (full) return;
        counter++;
        if (counter > fullCounter)
        {
            full = true;
            ScoreKeeper.Instance.IncrementScoring("Filled Nova's coffee");
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
