using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInteractionManager : MonoBehaviour {

    public bool isEnabled = true;
    public float dragDistance = 1.5f;
    public float raycastDistance = 3f;

    InputAction click;

    bool isPressed = false;
    Vector2 oldMousePos = Vector2.zero;

    GameObject mainCameraObj;
    Camera mainCamera;


    private void Start() {
        mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = mainCameraObj.GetComponent<Camera>();

        click = InputSystem.actions.FindAction("Click", true);
        click.started += ctx => mouseClick();
        click.canceled += ctx => mouseRelease();
    }


    private void Update() {
        if (isPressed) {
            mouseDrag();
        }
    }

    public bool raycastToMouse(out iItemInteraction iScript, out Vector3 mouseWorldspace) {
        iScript = null;
        mouseWorldspace = Vector3.zero;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray raycast = mainCamera.ScreenPointToRay(mousePos);

        Debug.DrawRay(raycast.origin, raycast.direction * raycastDistance, Color.wheat, 0.1f);
        if (Physics.Raycast(raycast, out RaycastHit hitInfo, raycastDistance, LayerMask.GetMask("MouseInteractable"))) {
            GameObject obj = hitInfo.collider.gameObject;
            iScript = obj.GetComponent<iItemInteraction>();
            mouseWorldspace = raycast.GetPoint(dragDistance);
            if (iScript != null && mouseWorldspace != Vector3.zero) 
                return true;
            Debug.LogWarning("Failed to get iItemInteraction script from layerd object " + obj.name);
            return false;
        }
        return false;
    }

    void mouseClick() {
        isPressed = true;

        if (raycastToMouse(out iItemInteraction iScript, out Vector3 mouseWorldspace)) {
            iScript.click();
            snappedItem = iScript;
        }
    }

    void mouseRelease() {
        isPressed = false;

        if (raycastToMouse(out iItemInteraction iScript, out Vector3 mouseWorldspace)) {
            iScript.release();
            snappedItem = null;
        }
    }

    [SerializeField] iItemInteraction snappedItem;
    private void mouseDrag() {
        if (snappedItem != null)
            snappedItem.drag(getMouseWorldspace(dragDistance));
    }

    private Vector3 getMouseWorldspace(float distance) {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray raycast = mainCamera.ScreenPointToRay(mousePos);
        return raycast.GetPoint(distance);
    }
}
