using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PopUpUIHandler : MonoBehaviour
{
    public static PopUpUIHandler Instance;
    public UIDocument uIDocument;
    public float blinkInterval = 0.5f;

    private VisualElement root;
    private VisualElement container;
    private VisualElement blinkContainer;
    private Coroutine blinkRoutine;

    void Start()
    {
        // Only allow one level manager
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        root = uIDocument.rootVisualElement;

        container = root.Query<VisualElement>("PopUpContainer").First();
        blinkContainer = root.Query<VisualElement>("BlinkContainer").First();
        blinkContainer.style.display = DisplayStyle.None;

        HidePopUp();
    }

    // show popup
    public void ShowPopUp()
    {
        container.style.display = DisplayStyle.Flex;
    }

    // hide popup
    public void HidePopUp()
    {
        container.style.display = DisplayStyle.None;
    }

    public void ShowBlinkPopUp()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            blinkContainer.style.display = DisplayStyle.Flex;
            yield return new WaitForSeconds(blinkInterval);
            blinkContainer.style.display = DisplayStyle.None;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void HideBlinkPopUp()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }

        blinkContainer.style.display = DisplayStyle.None;
    }
}
