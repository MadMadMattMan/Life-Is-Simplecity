using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float MoveSpeed = 10f, RotationSpeed = 5f;
    public float maxRayDistance = 1.5f;
    private float yRotation = 0f;
    private float xRotation = 0f;
    public bool isActive = true;
    bool moving = false;
    bool pastmove = false;
    public InputAction interactAction;
    public InputAction placeCauldronAction;
    List<Interactable> interactables = new List<Interactable>();
    CauldronStack cauldronStack;
    TrashCan trashCan;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public FootstepSFX footstepSFX;
    private bool interacting;
    Interactable lastInteractable;
    RaycastHit hit;
    private void OnEnable()
    {
        interactAction.started += Interact;
        placeCauldronAction.started += InteractCauldron;
        placeCauldronAction.Enable();
        interactAction.Enable();
    }
    private void OnDisable()
    {
        interactAction.started -= Interact;
        placeCauldronAction.started -= InteractCauldron;
        placeCauldronAction.Disable();
        interactAction.Disable();
    }
    private void Start()
    {
        interacting = false;
        cc = GetComponent<CharacterController>();
        cauldronStack = FindAnyObjectByType<CauldronStack>();
        trashCan = FindAnyObjectByType<TrashCan>();
        foreach (Interactable i in FindObjectsByType<Interactable>())
        {
            interactables.Add(i);
        }
    }
    public bool HasItem()
    {
        if (inventoryItem != null) return true;
        return false;
    }
    public void CarryItem(GameObject item)
    {
        inventoryItem = item;
        item.transform.SetParent(inventorySlot.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(0, 0, 0);
        item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public GameObject PlaceItem()
    {
        GameObject item = inventoryItem;
        item.transform.SetParent(null);
        inventoryItem = null;
        return item;
    }
    public void TrashItem()
    {
        Destroy(inventoryItem);
        inventoryItem = null;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (interacting)
        {
            if (lastInteractable == null) { interacting = false; return; }
            if (lastInteractable.IsTransitioning) return;
            lastInteractable.Interact();
            interacting = false;
            lastInteractable = null;
            return;
        }

        if (hit.transform != null && hit.transform.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact();
            if (interactable.beingInteractedWith)
            {
                lastInteractable = interactable;
                interacting = true;
            }
        }
    }

    public void InteractCauldron(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        Interactable interactable;
        if (hit.transform != null)
        {
            hit.transform.TryGetComponent(out interactable);
            if (interactable != null)
            {
                interactable.InteractCauldron();
            }
        }
    }

    private void Update()
    {
        if (moving != pastmove)
        {
            if (moving) footstepSFX.Play();
            else footstepSFX.Stop();
        }
        pastmove = moving;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Physics.Raycast(ray, out hit, maxRayDistance);
        if (hit.transform != null)
        {
            Interactable interactable;
            hit.transform.TryGetComponent(out interactable);
            if (interactable != null)
            {
                interactable.HoverInteraction();
            }
            else
            {
                foreach (Interactable i in interactables)
                {
                    i.UnhoverInteraction();
                }
            }
            CauldronStack stack = null;
            if (hit.transform.parent != null)
            {
                hit.transform.parent.TryGetComponent(out stack);
            }
            if (stack != null)
            {
                stack.HoverInteraction();
            }
            else
            {
                cauldronStack.UnhoverInteraction();
            }
            TrashCan trash = null;
            if (hit.transform.parent != null)
            {
                hit.transform.parent.TryGetComponent(out trash);
            }
            if (trash != null)
            {
                trash.HoverInteraction();
            }
            else
            {
                trashCan.UnhoverInteraction();
            }
        }
        else
        {
            foreach (Interactable i in interactables)
            {
                i.UnhoverInteraction();
            }
            cauldronStack.UnhoverInteraction();
            trashCan.UnhoverInteraction();
        }
    }
    public void Move(Vector2 dir)
    {
        if (!isActive) return;
        Vector3 move = transform.forward * dir.y + transform.right * dir.x;
        if (move.magnitude > 0.1f)
        {
            moving = true;
            move = move * MoveSpeed * Time.deltaTime;
            Vector3 moveDirection = new Vector3(move.x, 0f, move.z);
            cc.Move(moveDirection);
        }
        else moving = false;
    }
    public void Rotate(Vector2 dir)
    {
        if (!isActive) return;
        yRotation += dir.x * RotationSpeed * Time.deltaTime;
        xRotation -= dir.y * RotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
