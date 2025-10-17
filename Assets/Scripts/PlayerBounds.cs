using System;
using UnityEngine;

/// <summary>
/// This script is intended to be used on a bounding box surrounding a player environment.
/// It will teleport any collider tagged with "Player" that enters the trigger to the teleport destination.
/// </summary>
public class PlayerBounds : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;

    // If teleportDestination isn't set, default to finding the player 1 spawn anchor
    public void Awake()
    {
        if (teleportDestination == null)
        {
            // TODO change this to be player based player knows their spawn anchor
            var spawnAnchor = GameObject.FindGameObjectWithTag("PlayerSpawnAnchor");
            if (spawnAnchor == null)
            {
                throw new Exception("Teleport Destination not set and PlayerSpawnAnchor not found");
            }
            teleportDestination = spawnAnchor.transform;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            Debug.Log("Player entered player bounding box. Moving to position " + teleportDestination.position + " from current " + other.transform.position);
            other.transform.position = teleportDestination.position;
            controller.enabled = true;
        }
    }
}
