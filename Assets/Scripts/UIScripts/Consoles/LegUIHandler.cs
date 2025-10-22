using UnityEngine;

public class LegUIHandler : ConsoleUIHandler
{
    public static LegUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
