using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // [SerializeField] private GameObject hudPrefab;
    public float reach = 1f;
    private Camera fpsCam;
    Interactable currentItem;
    [SerializeField] private HUDController hUDController;

    private InputAction _interactAction;

    private InputAction _returnAction;

    private Interactable interacting;
    private int playerId;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        _interactAction = input.actions.FindAction("Interact");
        _returnAction = input.actions.FindAction("Return");
        hUDController = FindFirstObjectByType<HUDController>();
        fpsCam = GetComponent<PlayerInput>().camera;

        interacting = null;

        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex + 1;

        if (playerId == 2)
        {
            hUDController.playText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();

        // Debug.Log("Current Item: " + currentItem);
        // Debug.Log("Current Item: " + interacting);
        if (_interactAction.WasPressedThisFrame() && currentItem != null && interacting == null)
        {
            Debug.Log("Interacting with " + currentItem);
            currentItem.Interact(gameObject);
            interacting = currentItem;
        }

        if (_returnAction.WasPressedThisFrame() && interacting != null)
        {
            interacting.Return(gameObject);
            interacting = null;
        }

    }

    void CheckInteraction()
    {
        RaycastHit hit;

        Ray ray = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        if (Physics.Raycast(ray, out hit, reach))
        {
            if (hit.collider.tag == "interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentItem && newInteractable != currentItem)
                {
                    currentItem.DisableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrInteractable(newInteractable);
                }
                else
                {
                    DisableCurrInteractable();
                }
            }
            else
            {
                DisableCurrInteractable();
            }
        }
        else
        {
            DisableCurrInteractable();
        }
    }

    void SetNewCurrInteractable(Interactable newInteractable)
    {
        currentItem = newInteractable;
        currentItem.EnableOutline();
        hUDController.EnableInteractionText(playerId, currentItem.message);
    }

    void DisableCurrInteractable()
    {
        hUDController.DisableInteractionText(playerId);
        if (currentItem)
        {
            currentItem.DisableOutline();
            currentItem = null;
        }
    }

    public void NullInteracting()
    {
        interacting.Return(gameObject);
        interacting = null;
    }
}
