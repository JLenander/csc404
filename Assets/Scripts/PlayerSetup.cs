using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    private PlayerInput playerInput;
    public int playerId;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex + 1;

        int layer = LayerMask.NameToLayer("Player" + playerId);

        // Put renderers on correct layer
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.gameObject.layer = layer;

        // Setup that player’s camera (if it has one)
        Camera cam = GetComponentInChildren<Camera>();
        if (cam != null)
            cam.cullingMask &= ~(1 << layer);
    }
}
