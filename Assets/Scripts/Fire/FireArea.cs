using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Keeps a list of fire game objects and actives them, also keeps track of the state
/// </summary>
public class FireArea : MonoBehaviour
{
    [SerializeField] private string areaName;
    [SerializeField] private List<Fire> fires = new List<Fire>();
    private bool active;
    private int fireCount;
    private Collider detectCollider;
    private Coroutine enableRoutine;

    private List<PlayerSetup> playersInside = new List<PlayerSetup>();
    // private bool stopIncreasing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        active = false;

        // disable all fires
        foreach (Fire fire in fires)
        {
            fire.fireArea = this;
        }

        fireCount = 0;

        detectCollider = GetComponent<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            PlayerSetup playerSetup = other.GetComponent<PlayerSetup>();

            if (playerSetup != null && !playersInside.Contains(playerSetup))
            {
                playersInside.Add(playerSetup);
                playerSetup.extinguisher.ActivateExtinguisher(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            PlayerSetup playerSetup = other.GetComponent<PlayerSetup>();
            if (playerSetup != null)
            {
                playersInside.Remove(playerSetup);
                playerSetup.extinguisher.ActivateExtinguisher(false);
            }
        }
    }

    public void EnableFires()
    {
        if (active) return;

        active = true;
        // stopIncreasing = false;

        if (enableRoutine != null)
        {
            StopCoroutine(enableRoutine);
        }

        enableRoutine = StartCoroutine(EnableRoutine());
    }

    // gradually increase fires
    IEnumerator EnableRoutine()
    {
        fireCount = 0;

        foreach (Fire fire in fires)
        {
            // if (stopIncreasing) yield break;

            fire.StartFire();
            fireCount++;

            yield return new WaitForSeconds(2);
        }
    }

    // used by Fire to tell manager fire is put out
    public void DisableFire()
    {
        // reduce fire count
        fireCount--;
        // stopIncreasing = true;

        // if count is 0, fire is put out
        if (fireCount <= 0)
        {
            // notify fire manager area is safe
            active = false;

            foreach (PlayerSetup playerSetup in playersInside)
            {
                if (playerSetup != null && playerSetup.extinguisher != null)
                    playerSetup.extinguisher.ActivateExtinguisher(false);
            }

            playersInside.Clear();

            if (enableRoutine != null)
            {
                StopCoroutine(enableRoutine);
                enableRoutine = null;
            }

            FireManager.Instance.StopFireArea(areaName);
        }
    }
}
