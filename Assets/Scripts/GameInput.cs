using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteraction;

    PlayerInputActions playerInputActions;

    public Vector2 GetMovementVector()
    {
        return playerInputActions.Gameplay.Movement.ReadValue<Vector2>();
    }

    private void Awake()
    {
        if (null != Instance)
        {
            Debug.LogError("GameInput instance already exists");
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Gameplay.Interaction.performed += 
            ctx => OnInteraction?.Invoke(this, EventArgs.Empty);
    }
}
