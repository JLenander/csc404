using UnityEngine;

public class HeadUIHandler : ConsoleUIHandler
{
    public static HeadUIHandler Instance;
    public void Awake()
    {
        Instance = this;
    }
}
