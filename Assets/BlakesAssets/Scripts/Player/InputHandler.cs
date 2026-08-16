using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerController controller;
    private InputAction MoveAction, LookAction;
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        LookAction = InputSystem.actions.FindAction("Look");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 movementVector = MoveAction.ReadValue<Vector2>();
        controller.Move(movementVector);

        Vector2 lookVector = LookAction.ReadValue<Vector2>();
        controller.Rotate(lookVector);
    }
}
