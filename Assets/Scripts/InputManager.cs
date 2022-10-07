using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    // public should be properties 
    [HideInInspector] public Vector2 move { get; private set; }
    [HideInInspector] public Vector2 look { get; private set; }
    [HideInInspector] public bool run { get; private set; }
    [HideInInspector] public bool jump { get; private set; }

    private InputActionMap actionMap;
    // actions
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction jumpAction;

    private void Awake() 
    {
        HideCursor();
        actionMap = playerInput.currentActionMap;

        moveAction = actionMap.FindAction("Move");
        lookAction = actionMap.FindAction("Look");
        runAction = actionMap.FindAction("Run");
        jumpAction = actionMap.FindAction("Jump");

        moveAction.performed += onMove;
        lookAction.performed += onLook;
        runAction.performed += onRun;
        jumpAction.performed += onJump;

        moveAction.canceled += onMove;
        lookAction.canceled += onLook;
        runAction.canceled += onRun;
        jumpAction.canceled += onJump;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void onMove(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();

    private void onLook(InputAction.CallbackContext context) => look = context.ReadValue<Vector2>();

    private void onRun(InputAction.CallbackContext context) => run = context.ReadValueAsButton();

    private void onJump(InputAction.CallbackContext context) => jump = context.ReadValueAsButton();

    private void OnEnable() => actionMap.Enable();
    
    private void OnDisable() => actionMap.Disable();
    
}