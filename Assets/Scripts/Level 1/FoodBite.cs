using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FoodBite : InteractableObject, IPooledObject
{
    private Food _foodBiteSpawner;
    private Transform ogParent;
    private Rigidbody rb;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform popUp;
    [SerializeField] private Transform graphic;
    public float floatSpeed = 1f;
    public int score = 3;

    public override void Start()
    {
        base.Start();
        popUp.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }

    public void OnSpawn()
    {
        popUp.gameObject.SetActive(false);
        graphic.gameObject.SetActive(true);
        gameObject.SetActive(true);
        ogParent = transform.parent;

        rb = GetComponent<Rigidbody>();
    }

    public override void InteractWithHand(Transform obj, HandMovement target)
    {
        if (canInteract && canPickup)
        {
            // move to hand
            DisableOutline();
            transform.parent = obj;
            transform.localPosition = new Vector3(0.0f, 5.2f, -1.0f);
            transform.localRotation = Quaternion.Euler(-88f, 10f, 0f);
            canPickup = false;

            rb.isKinematic = true;
            Debug.Log("pickup success");

            target.handAnimator.SetTrigger("Hold"); // sets current hand to hold anim
            target.SetTargetCurrentObject(this);
        }
    }

    public override void StopInteractWithHand(HandMovement target)
    {
        // return to original position
        transform.parent = ogParent;
        canPickup = true;
        rb.isKinematic = false;
        target.handAnimator.SetTrigger("Neutral"); // sets the current hand back to neutral
    }

    // TODO: write a function to check if it hit a bag area
    // if it drops into the bag then score points and destroy, also set foodBiteSpawner.biteInScene to false

    public void SetFoodBiteSpawner(Food foodBiteSpawner)
    {
        _foodBiteSpawner = foodBiteSpawner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Hit ground!");
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = collision.contacts[0].point + Vector3.up * 0.01f;
        }
        else if (collision.gameObject.CompareTag("Bag")) {
            Debug.Log("eating points secured");
            ScoreKeeper.Instance.ModifyScore(score);
            StartCoroutine(DisappearRoutine());
        }
    }

    IEnumerator DisappearRoutine()
    {
        float duration = 2f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        rb.isKinematic = true;
        popUp.LookAt(_foodBiteSpawner.robotHead);
        popUp.Rotate(0f, 180f, 0f);
        popUp.gameObject.SetActive(true);
        graphic.gameObject.SetActive(false);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Move upward
            popUp.position = startPos + Vector3.up * (elapsed * floatSpeed);

            yield return null;
        }

        // disable the object (delayed to play animation)
        rb.isKinematic = false;
        gameObject.SetActive(false);
    }
}
