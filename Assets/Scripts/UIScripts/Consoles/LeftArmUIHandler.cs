using UnityEngine;

public class LeftArmUIHandler : ConsoleUIHandler
{
    public static LeftArmUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
