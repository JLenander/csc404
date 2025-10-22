using UnityEngine;

public class RightArmUIHandler : ConsoleUIHandler
{
    public static RightArmUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
