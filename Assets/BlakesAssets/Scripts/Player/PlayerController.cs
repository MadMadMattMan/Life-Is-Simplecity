using UnityEngine;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float MoveSpeed = 10f, RotationSpeed = 5f;
    public float maxRayDistance = 1.5f;
    private float yRotation = 0f;
    private float xRotation = 0f;
    public bool isActive = true;
    List<Interactable> interactables = new List<Interactable>();
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        foreach (Interactable i in FindObjectsByType<Interactable>())
        {
            interactables.Add(i);
        }
    }
    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, maxRayDistance))
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
        }
        else
        {
            foreach (Interactable i in interactables)
            {
                i.UnhoverInteraction();
            }
        }
    }
    public void Move(Vector2 dir)
    {
        if (!isActive) return;
        Vector3 move = transform.forward * dir.y + transform.right * dir.x;
        move = move * MoveSpeed * Time.deltaTime;
        Vector3 moveDirection = new Vector3(move.x, 0f, move.z);
        cc.Move(moveDirection);
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
