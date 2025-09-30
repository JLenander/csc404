using UnityEngine;

public class Evidence : InteractableObject, IPooledObject
{
    public int score = 1; // 1 - 3, or 10 depending on obj
    private EvidenceSpawner _evidenceSpawner;

    public void OnSpawn()
    {
        // enable object
        gameObject.SetActive(true);
    }

    public override void InteractWithHand(Transform obj, HandMovement target)
    {
        // disintegrate it (play animation)

        // give a score
        ScoreKeeper.Instance.ModifyScore(score);

        // cancel interaction so it doesnt think we're holding it (maybe set timer later)
        target.StopInteractingWithObject(this);

        // tell spawner it was dealt
        _evidenceSpawner.ReduceCount();

        // disable the object (delayed when we add some animation)
        gameObject.SetActive(false);
    }

    public void SetEvidenceSpawner(EvidenceSpawner evidenceSpawner)
    {
        _evidenceSpawner = evidenceSpawner;
    }

    // public override void StopInteractWithHand(HandMovement target)
    // {
    //     // nothing
    // }
}
