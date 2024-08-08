using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public float movementSpeed = 5f;

    private Vector2 movement = Vector2.zero;

    private Rigidbody2D rb2d;

    private PlayerInput playerInput;
    private InputAction movementAction;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        playerInput = GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Movement"];
    }

    private void OnEnable()
    {
        // Enable movement callbacks
        movementAction.Enable();
        movementAction.started += OnMovement;
        movementAction.performed += OnMovement;
        movementAction.canceled += OnMovement;
    }

    private void OnDisable()
    {
        // Disable movement callbacks
        movementAction.Disable();
        movementAction.started -= OnMovement;
        movementAction.performed -= OnMovement;
        movementAction.canceled -= OnMovement;
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement * Time.fixedDeltaTime);
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        input.Normalize();
        movement = input * movementSpeed;
    }
}
