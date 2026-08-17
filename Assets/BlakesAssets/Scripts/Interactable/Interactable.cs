using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class Interactable : MonoBehaviour
{
    [Header("Inherited")]
    [SerializeField] private Camera cam;
    private bool interactable;
    [HideInInspector] public bool beingInteractedWith;
    [HideInInspector] public bool playerLock = false;
    public bool canPlaceCauldron = true;
    public GameObject Cauldron;
    public Transform CauldronParent;
    public GameObject interactionPopup;
    public Transform camTarget;
    public InputAction interactAction;
    public InputAction placeCauldronAction;
    public AudioSource audioSource;
    public AudioClip MetalClunk;
    public AudioClip Whoosh;
    private float smoothTime = 0.3f;
    private float degreesPerSecond = 360.0f;
    private Transform camParent;
    private Vector3 currentVelocity = Vector3.zero;
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
    public virtual void PlaceCauldron()
    {
        Cauldron = PlayerController.Instance.PlaceItem();
        Cauldron.transform.parent = CauldronParent;
        Cauldron.transform.localPosition = Vector3.zero;
        Cauldron.transform.rotation = Quaternion.Euler(Vector3.zero);
        Cauldron.transform.localScale = Vector3.one;
        audioSource.PlayOneShot(MetalClunk);
    }
    public void InteractCauldron(InputAction.CallbackContext context)
    {
        if (context.started && interactable)
        {
            if (canPlaceCauldron)
            {
                if (Cauldron != null)
                {
                    if (!PlayerController.Instance.HasItem()) 
                    {
                        PickupCauldron();
                    }
                    else
                    {
                        PopupManager.Instance.CreatePopup("Cannot place another cauldron here!");
                    }
                }
                else
                {
                    if (PlayerController.Instance.HasItem())
                    {
                        PlaceCauldron();
                    }
                    else
                    {
                        PopupManager.Instance.CreatePopup("Nothing to place!");
                    }
                }
            }
            else
            {
                PopupManager.Instance.CreatePopup("Cant place a cauldron here!");
            }
        }
    }
    public virtual void PickupCauldron() {
        Cauldron.transform.SetParent(null);
        PlayerController.Instance.CarryItem(Cauldron);
        Cauldron = null;
        audioSource.PlayOneShot(MetalClunk);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started && beingInteractedWith && !playerLock)
        {
            OnUninteract();
            StartCoroutine(nameof(UninteractionSequence));
        }
        if (context.started && interactable)
        {
            if (canPlaceCauldron)
            {
                bool playerHasItem = PlayerController.Instance.HasItem();
                if (playerHasItem && Cauldron == null)
                {
                    PlaceCauldron();
                    PlayerController.Instance.isActive = false;
                    camParent = cam.transform.parent;
                    cam.transform.SetParent(null);
                    StartCoroutine(nameof(InteractionSequence));
                }
                else if (playerHasItem && Cauldron != null)
                {
                    PopupManager.Instance.CreatePopup("Cant interact whilst carring cauldron!");
                }
                else
                {
                    PlayerController.Instance.isActive = false;
                    camParent = cam.transform.parent;
                    cam.transform.SetParent(null);
                    StartCoroutine(nameof(InteractionSequence));
                }
            }
            else
            {
                if (!beingInteractedWith)
                {
                    bool playerHasItem = PlayerController.Instance.HasItem();
                    if (playerHasItem)
                    {
                        PopupManager.Instance.CreatePopup("Cant interact whilst carring cauldron!");
                    }
                    else
                    {
                        PlayerController.Instance.isActive = false;
                        camParent = cam.transform.parent;
                        cam.transform.SetParent(null);
                        StartCoroutine(nameof(InteractionSequence));
                    }
                }
            }
        }
    }
    public IEnumerator InteractionSequence()
    {
        audioSource.PlayOneShot(Whoosh);
        while ((cam.transform.position - camTarget.position).sqrMagnitude > 0.01f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camTarget.position, ref currentVelocity, smoothTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, camTarget.rotation, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
        OnInteract();
    }
    public IEnumerator UninteractionSequence()
    {
        audioSource.PlayOneShot(Whoosh);
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
