using UnityEngine;
using UnityEngine.UIElements;

public class BrainUIHandler : OverlayUIHandler
{
    public static BrainUIHandler Instance;

    private VisualElement doorUI;
    private VisualElement taskUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Instance = this;
        doorUI = root.Query<VisualElement>("DoorWindow").First();
        taskUI = root.Query<VisualElement>("TaskWindow").First();
    }
}
