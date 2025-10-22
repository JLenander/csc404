using UnityEngine;

public class LegUIHandler : OverlayUIHandler
{
    public static LegUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
