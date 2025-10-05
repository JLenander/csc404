using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Food : InteractableObject
{
    // spawn food bites

    public int totalBites = 10;
    private int foodBiteCount;
    private ObjectPooler objectPooler;
    public Transform robotHead;

    public override void Start()
    {
        base.Start();
        objectPooler = ObjectPooler.Instance;
        foodBiteCount = 0;
    }

    public override void InteractWithHand(Transform wrist, HandMovement target)
    {
        if (foodBiteCount < totalBites && canInteract)
        {
            // spawn a food bite from the object pooler
            GameObject foodBiteObj = objectPooler.SpawnFromPool("FoodBite", transform.position, transform.rotation);

            FoodBite foodBite = foodBiteObj.GetComponent<FoodBite>();
            if (foodBite != null)
            {
                foodBite.SetFoodBiteSpawner(this);
            }
            target.StopInteractingWithObject(this);
            target.InteractWithObject(foodBite);

            foodBiteCount++;
            Debug.Log(foodBiteCount);
            // TODO: reduce size or change animation state based on numbites
        }
        else
        {
            Debug.Log("No more food bites!");
            target.StopInteractingWithObject(this);
            canInteract = false;
        }
    }
}
