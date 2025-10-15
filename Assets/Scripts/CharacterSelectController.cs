using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Outline _outline;
    [SerializeField] private TMP_Text floatingLabel;
    [SerializeField] private TMP_Text numberLabel;

    public float rotationSpeed = 90f;   // degrees per second
    private Vector2 currentRotationInput = Vector2.zero;
    
    private int _playerIndex;
    
    public void Init(PlayerInput input, int index)
    {
        _playerInput = input;
        _playerIndex = index;
        _outline = GetComponentInChildren<Outline>();
        numberLabel.text = "Player " + (_playerIndex + 1);
        
        // Subscribe to Vector2 input
        _playerInput.actions["Rotate"].performed += ctx => Rotate(ctx.ReadValue<Vector2>());
        _playerInput.actions["Rotate"].canceled += ctx => Rotate(Vector2.zero);
    }

    private void Start()
    {
        if (IsCharacterSelectScene)
        {
            // Disable the prefab's camera so the scene camera is used
            Camera[] cameras = GetComponentsInChildren<Camera>();
            foreach (var cam in cameras)
                cam.enabled = false;
            
            GetComponent<Player>().enabled = false;
            GetComponent<PlayerInteract>().enabled = false;

        }
    }

    private bool IsCharacterSelectScene => SceneManager.GetActiveScene().name == "CharacterSelect";

    public void Rotate(Vector2 input)
    {
        currentRotationInput = input;
    }

    private void Update()
    {
        // Rotate around Y-axis using x input
        transform.Rotate(Vector3.up * currentRotationInput.x * rotationSpeed * Time.deltaTime);
    }

    public void ChangeColor(Color newColor)
    {
        _outline.OutlineColor = newColor;
        numberLabel.color = newColor;
    }

    public void ToggleReady(bool isReady)
    {
        numberLabel.text = isReady ? "Player " + (_playerIndex + 1) + " Ready!" : "Player " + (_playerIndex + 1);
    }
    
    public void ShowLabel(string message)
    {
        floatingLabel.text = message;
        floatingLabel.color = Color.red;
        floatingLabel.gameObject.SetActive(true);
    }
    
    public void HideLabel()
    {
        floatingLabel.gameObject.SetActive(false);
    }
    
    // private void OnDestroy()
    // {
    //     if (_playerInput != null)
    //     {
    //         var actions = _playerInput.actions;
    //         if (actions != null)
    //         {
    //             actions["Rotate"].performed -= ctx => Rotate(ctx.ReadValue<Vector2>());
    //             actions["Rotate"].canceled -= ctx => Rotate(Vector2.zero);
    //         }
    //     }
    // }


}