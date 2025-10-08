using UnityEngine;

public class FillCup : MonoBehaviour
{
    [SerializeField] private Transform liquid;
    [SerializeField] private int fullCounter;
    [SerializeField] private float maxFillHeight = 1f;
    [SerializeField] private float initialHeight = 0.1f;

    private int counter;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = new Vector3(4f, initialHeight, 4f);
        liquid.localScale = baseScale;
        liquid.localPosition = Vector3.zero;
        counter = 0;
    }

    public void AddCoffee()
    {
        counter = Mathf.Min(counter + 1, fullCounter);

        float fillProgress = (float)counter / fullCounter;
        float newYScale = Mathf.Lerp(initialHeight, maxFillHeight, fillProgress);
        float yOffset = (newYScale - initialHeight) / 1.5f;

        liquid.localScale = new Vector3(baseScale.x, newYScale, baseScale.z);
        liquid.localPosition = new Vector3(0f, yOffset, 0f);
    }
}
