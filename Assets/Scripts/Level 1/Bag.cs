using UnityEngine;

public class Bag : MonoBehaviour
{
    public Outline outline;

    private void Start()
    {
        DisableOutline();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}
