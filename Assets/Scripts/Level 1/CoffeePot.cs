using UnityEngine;

public class CoffeePot : InteractableObject
{
    public Collider triggerCollider;
    private Quaternion ogRotation;
    private Transform ogParent;
    private Rigidbody rb;

    public float rayLength = 50f;
    public float pourThresholdAngle = 30f; // degrees below horizontal

    private bool isPouring = false;

    [SerializeField] private Transform spoutTip;          // assign in Inspector
    [SerializeField] private Transform coffeePour;
    private ParticleSystem coffeePourEffect;


    public override void Start()
    {
        base.Start();
        ogParent = transform.parent;
        ogRotation = transform.localRotation;

        rb = GetComponent<Rigidbody>();
        
        coffeePourEffect = coffeePour.GetComponent<ParticleSystem>();
        Debug.Log(coffeePourEffect);
        coffeePourEffect.Stop(); // don’t play immediately
    }
    private void Update()
    {
        Vector3 origin = spoutTip.position;
        Vector3 direction = transform.forward.normalized;

        // visualize the spout ray
        Debug.DrawRay(spoutTip.position, direction * rayLength, isPouring ? Color.green : Color.red);

        // is it tilted downward enough
        float downwardDot = Vector3.Dot(transform.forward, Vector3.down);
        bool pouringNow = downwardDot > Mathf.Cos(pourThresholdAngle * Mathf.Deg2Rad);

        // detect transition from not-pouring to pouring
        if (pouringNow && !isPouring)
        {
            isPouring = true;
            OnStartPour();
        }
        // detect transition from pouring to not-pouring
        else if (!pouringNow && isPouring)
        {
            isPouring = false;
            OnStopPour();
        }

        // optional raycast visualization
        if (isPouring)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength))
            {
                Debug.DrawLine(origin, hit.point, Color.cyan);
                if (hit.collider.CompareTag("Cup"))
                {
                    FillCup cup = hit.collider.GetComponent<FillCup>();
                    cup.AddCoffee();
                }
            }
        }
    }

    private void OnStartPour()
    {
        Debug.Log("Started pouring!");
        coffeePourEffect.Play();
    }

    private void OnStopPour()
    {
        Debug.Log("Stopped pouring!");
        coffeePourEffect.Stop();
    }

    public override void InteractWithHand(Transform obj, HandMovement target)
    {
        if (canInteract && canPickup)
        {
            // move to hand
            DisableOutline();
            transform.parent = obj;
            transform.localPosition = new Vector3(-4.5f, 6.0f, -2.0f);
            transform.localRotation = Quaternion.Euler(-8f, -100f, -45f);

            Debug.Log(transform.rotation);
            canPickup = false;

            target.SetWristRotation(new Vector3(0, 0, 0));

            rb.isKinematic = true;
            triggerCollider.enabled = false;
            Debug.Log("pickup success");

            target.SetTargetCurrentObject(this);
            target.handAnimator.SetTrigger("Hold"); // sets current hand to hold anim
        }
    }

    public override void StopInteractWithHand(HandMovement target)
    {
        // return to original position
        transform.parent = ogParent;
        Vector3 currPos = transform.localPosition;
        transform.localPosition = new Vector3(currPos.x, 5.14f, currPos.z);

        Quaternion currRotation = transform.localRotation;
        transform.localRotation = Quaternion.Euler(0f, currRotation.y, 0f);

        canPickup = true;
        triggerCollider.enabled = true;
        target.oppositeHandAnimator.SetTrigger("Neutral"); // sets the opposite hand back to neutral
    }
}
