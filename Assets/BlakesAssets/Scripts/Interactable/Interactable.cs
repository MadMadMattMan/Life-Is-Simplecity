using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class Interactable : MonoBehaviour
{
    [Header("Inherited")]
    private Camera cam;
    private bool interactable;
    public bool beingInteractedWith;
    public GameObject interactionPopup;
    public Transform camTarget;
    public InputAction interactAction;
    public InputAction uninteractAction;
    private float smoothTime = 0.3f;
    private float degreesPerSecond = 90.0f;
    private Transform camParent;
    private Vector3 currentVelocity = Vector3.zero;
    private void Start()
    {
        cam = Camera.main;
    }
    private void OnEnable()
    {
        interactAction.started += Interact;
        uninteractAction.started += Uninteract;
        uninteractAction.Enable();
        interactAction.Enable();
    }
    private void OnDisable()
    {
        interactAction.started -= Interact;
        uninteractAction.started -= Uninteract;
        uninteractAction.Disable();
        interactAction.Disable();
    }
    public void UnhoverInteraction()
    {
        interactionPopup.SetActive(false);
        interactable = false;
    }
    public void HoverInteraction()
    {
        interactionPopup.SetActive(true);
        interactable = true;
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started && interactable && !beingInteractedWith)
        {
            PlayerController.Instance.isActive = false;
            camParent = cam.transform.parent;
            cam.transform.SetParent(null);
            StartCoroutine(nameof(InteractionSequence));
        }
    }
    public void Uninteract(InputAction.CallbackContext context)
    {
        if (context.started && beingInteractedWith)
        {
            OnUninteract();
            StartCoroutine(nameof(UninteractionSequence));
        }
    }
    public IEnumerator InteractionSequence()
    {
        while((cam.transform.position - camTarget.position).sqrMagnitude > 0.01f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camTarget.position, ref currentVelocity, smoothTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, camTarget.rotation, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
        OnInteract();
    }
    public IEnumerator UninteractionSequence()
    {
        while ((cam.transform.position - camParent.position).sqrMagnitude > 0.01f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camParent.position, ref currentVelocity, smoothTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, camParent.rotation, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
        cam.transform.SetParent(camParent);
        beingInteractedWith = false;
        PlayerController.Instance.isActive = true;
    }
    public virtual void OnInteract()
    {
        beingInteractedWith = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public virtual void OnUninteract()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
