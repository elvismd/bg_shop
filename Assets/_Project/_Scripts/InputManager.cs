using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : SingleInstance<InputManager>
{
    [SerializeField]
    private PlayerInput _input;

    private Vector2 move;

    private InputActionMap _currentMap;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _pauseAction;

    public Vector2 Move => move;
    public InputAction Interact => _interactAction;
    public InputAction Pause => _pauseAction;

    protected override void Awake()
    {
        base.Awake();

        _currentMap = _input.currentActionMap;

        _moveAction = _currentMap.FindAction("Movement");
        _interactAction = _currentMap.FindAction("Interact");
        _pauseAction = _currentMap.FindAction("Pause");

        _moveAction.performed += OnMovement;
        _moveAction.canceled += OnMovement;

    }

    private void OnEnable()
    {
        _currentMap.Enable();
    }

    private void OnDisable()
    {
        _currentMap.Disable();
    }

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }
}
