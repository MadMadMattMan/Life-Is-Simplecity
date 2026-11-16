using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerController controller;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction MoveAction, LookAction;
    void Start()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        LookAction = InputSystem.actions.FindAction("Look");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void RandomKeybinds()
    {
        string newMap = null;
        while (newMap == null || newMap == "UI")
        {
            newMap = inputActions.actionMaps[Random.Range(0, inputActions.actionMaps.Count - 1)].name;
        }
        Debug.Log("Switching to control preset: " + newMap);
        InputActionMap actionMap = inputActions.FindActionMap(newMap);
        actionMap.Enable();
        MoveAction = actionMap.FindAction("Move");
        LookAction = actionMap.FindAction("Look");
        MouseInteractionManager.click = actionMap.FindAction("Click");
    }
    void Update()
    {
        Vector2 movementVector = MoveAction.ReadValue<Vector2>();
        controller.Move(movementVector);

        Vector2 lookVector = LookAction.ReadValue<Vector2>();
        controller.Rotate(lookVector);
    }
    public static InputHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
