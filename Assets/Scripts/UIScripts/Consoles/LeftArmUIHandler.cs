using UnityEngine;

public class LeftArmUIHandler : OverlayUIHandler
{
    public static LeftArmUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
