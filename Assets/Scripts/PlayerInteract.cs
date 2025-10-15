using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // [SerializeField] private GameObject hudPrefab;
    public float reach = 1f;
    private Camera fpsCam;
    Interactable currentItem;

    private InputAction _interactAction;
    private InputAction _returnAction;

    private Interactable interacting;
    private int playerId;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        _interactAction = input.actions.FindAction("Interact");
        _returnAction = input.actions.FindAction("Return");
        fpsCam = GetComponent<PlayerInput>().camera;

        interacting = null;

        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;

    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();

        if (_interactAction.WasPressedThisFrame() && currentItem != null && interacting == null)
        {
            if (currentItem.CanInteract())
            {
                Debug.Log("Interacting with " + currentItem);
                interacting = currentItem;
                currentItem.Interact(gameObject);
                DisableCurrInteractable();

                if (GlobalPlayerUIManager.Instance != null)
                {
                    GlobalPlayerUIManager.Instance.EnableScreenGreyscale(playerId);
                }
            }
        }

        if (_returnAction.WasPressedThisFrame() && interacting != null)
        {
            interacting.Return(gameObject);
            interacting = null;
            if (GlobalPlayerUIManager.Instance != null)
                GlobalPlayerUIManager.Instance.DisableScreenGreyscale(playerId);
        }

    }

    void CheckInteraction()
    {
        if (interacting != null)
        {
            SetReturnText(interacting);
            return;
        }

        RaycastHit hit;

        Ray ray = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        if (Physics.Raycast(ray, out hit, reach))
        {
            if (hit.collider.tag == "interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                // Null interact
                if (!newInteractable)
                {
                    return;
                }

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

    void SetReturnText(Interactable currItem)
    {
        if (GlobalPlayerUIManager.Instance != null)
            GlobalPlayerUIManager.Instance.EnableInteractionText(playerId, "To Return", currItem.msgColour);
    }

    void SetNewCurrInteractable(Interactable newInteractable)
    {
        currentItem = newInteractable;
        currentItem.EnableOutline();

        if (GlobalPlayerUIManager.Instance != null)
            GlobalPlayerUIManager.Instance.EnableInteractionText(playerId, currentItem.hoverMessage, currentItem.msgColour);
    }

    void DisableCurrInteractable()
    {
        if (GlobalPlayerUIManager.Instance != null)
            GlobalPlayerUIManager.Instance.DisableInteractionText(playerId);

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
