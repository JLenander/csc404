using UnityEngine;

public class ConsoleControlsUI : OverlayUIHandler
{
    public static ConsoleControlsUI Instance;

    public void Awake()
    {
        Instance = this;
    }
}
