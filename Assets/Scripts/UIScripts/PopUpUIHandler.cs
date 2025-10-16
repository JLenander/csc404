using UnityEngine;
using UnityEngine.UIElements;

public class PopUpUIHandler : MonoBehaviour
{
    public static PopUpUIHandler Instance;
    public UIDocument uIDocument;

    private VisualElement root;
    private VisualElement container;

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
}
