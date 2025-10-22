using UnityEngine;

public class RightArmUIHandler : OverlayUIHandler
{
    public static RightArmUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
