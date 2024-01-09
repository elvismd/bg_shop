using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;

    private Vector2 move;
    private InputActionMap _currentMap;
    private InputAction _moveAction;

    public Vector2 Move => move;

    private void Awake()
    {
        _currentMap = _input.currentActionMap;

        _moveAction = _currentMap.FindAction("Movement");
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
        Debug.Log("eee");
    }
}
