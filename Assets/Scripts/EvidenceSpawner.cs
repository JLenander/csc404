using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the evidence and shoots it, might be more than 1 in the scene
/// </summary>
public class EvidenceSpawner : MonoBehaviour
{
    public int maxEvidence = 5;
    public float spawnInterval = 5f;
    public Transform[] spawnAnchors;
    public float launchForce = 10f;
    public Transform robotHead;
    public AudioSource audioSource;

    public GameObject uniqueEvidence;
    public Transform uniqueAnchor;
    private float spawnTimer;
    private int evidenceCount; // keep track of num evidences in scene
    private ObjectPooler objectPooler;
    private string[] evidenceTypes = { "Notepad", "Map", "Polaroid" }; // types of evidence
    private bool disabled = true;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        evidenceCount = 0;
        spawnTimer = spawnInterval; // start the timer
        uniqueEvidence.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // only spawn 5 evidences max
        if (evidenceCount < maxEvidence && !disabled) // make sure it is less than the max
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEvidence();

                spawnTimer = spawnInterval; // reset timer
            }
        }
    }

    public void SpawnTempSpecial()
    {
        if (audioSource != null)
            audioSource.Play();

        uniqueEvidence.SetActive(true);
        uniqueEvidence.transform.position = uniqueAnchor.position;
        Evidence evidence = uniqueEvidence.GetComponent<Evidence>();
        evidence.SetEvidenceSpawner(this);
        Rigidbody rb = uniqueEvidence.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // reset before applying force
            rb.AddForce(uniqueAnchor.forward * launchForce, ForceMode.Impulse);
        }
    }

    void SpawnEvidence()
    {
        if (audioSource != null)
            audioSource.Play();
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
        if (disabled)
        {
            disabled = false;
        }
    }
}
