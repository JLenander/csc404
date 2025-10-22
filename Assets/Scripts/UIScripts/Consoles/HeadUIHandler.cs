using UnityEngine;

public class HeadUIHandler : OverlayUIHandler
{
    public static HeadUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
