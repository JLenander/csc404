using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the evidence and shoots it, might be more than 1 in the scene
/// </summary>
public class EvidenceSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxEvidence = 5;
    public float spawnInterval = 5f;
    public Transform[] spawnAnchors;
    public float launchForce = 10f;
    private float spawnTimer;
    private int evidenceCount; // keep track of num evidences in scene
    private ObjectPooler objectPooler;
    private string[] evidenceTypes = { "Notepad", "Map", "Polaroid" }; // types of evidence
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        evidenceCount = 0;
        spawnTimer = spawnInterval; // start the timer
    }

    // Update is called once per frame
    void Update()
    {
        // only spawn 5 evidences max
        if (evidenceCount < maxEvidence) // make sure it is less than the max
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEvidence();

                spawnTimer = spawnInterval; // reset timer
            }
        }
    }

    void SpawnEvidence()
    {
        // pick a random evidence
        string randomType = evidenceTypes[Random.Range(0, evidenceTypes.Length)];

        // pick a random anchor
        Transform anchor = spawnAnchors[Random.Range(0, spawnAnchors.Length)];

        GameObject evidenceObj = objectPooler.SpawnFromPool(randomType, anchor.position, anchor.rotation);

        Evidence evidence = evidenceObj.GetComponent<Evidence>();

        // give reference to spawn to evidence
        if (evidence != null)
        {
            evidence.SetEvidenceSpawner(this);
        }

        // get rb refernece to launch
        Rigidbody rb = evidenceObj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // reset before applying force
            rb.AddForce(anchor.forward * launchForce, ForceMode.Impulse);
        }


        evidenceCount++;
    }

    public void ReduceCount()
    {
        evidenceCount--;
    }
}
