using UnityEngine;

// Temp script to put the interaction point on the hand
public class Hand : MonoBehaviour
{
    [SerializeField] private HandMovement handMovement;

    public HandMovement GetHandMovement()
    {
        return handMovement;
    }
}
