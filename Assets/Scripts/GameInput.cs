using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractNpcAction;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAtlernate.performed += InteractAtlernate_performed;
        inputActions.Player.InteractNPC.performed += InteractNPC_performed; ;
    }

    private void InteractNPC_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractNpcAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAtlernate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetNormalizedMovements()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
