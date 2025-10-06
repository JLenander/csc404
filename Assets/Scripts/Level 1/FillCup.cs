using JetBrains.Annotations;
using UnityEngine;

public class FillCup : MonoBehaviour
{
    private int counter;
    private int fullCounter;
    void Start()
    {
        counter = 0;
    }

    public void AddCoffee()
    {
        if (counter < fullCounter) counter++;
    }
}
