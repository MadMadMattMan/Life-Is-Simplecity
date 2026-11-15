using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    [Header("Inherited")]
    private bool interactable;
    [HideInInspector] public bool beingInteractedWith;
    [HideInInspector] public bool playerLock = false;
    public bool canPlaceCauldron = true;
    public GameObject Cauldron;
    public Transform CauldronParent;
    public GameObject interactionPopup;
    public Transform camTarget;
    public UnityEvent interactAction;
    public UnityEvent placeCauldronAction;
    public AudioSource audioSource;
    public AudioClip MetalClunk;
    public AudioClip Whoosh;
    private float smoothTime = 0.3f;
    private float degreesPerSecond = 360.0f;
    private Transform camParent;
    private Vector3 currentVelocity = Vector3.zero;
    public bool IsTransitioning => state == State.Entering || state == State.Exiting;
    private enum State { Idle, Entering, Active, Exiting }
    private State state = State.Idle;
    private void Start()
    {
        camParent = Camera.main.transform.parent;
    }
    private void OnEnable()
    {
        interactAction.AddListener(Interact);
        placeCauldronAction.AddListener(InteractCauldron);
    }
    private void OnDisable()
    {
        interactAction.RemoveListener(Interact);
        placeCauldronAction.RemoveListener(InteractCauldron);
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
    public void InteractCauldron()
    {
        if (interactable)
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
    public virtual void PickupCauldron()
    {
        Cauldron.transform.SetParent(null);
        PlayerController.Instance.CarryItem(Cauldron);
        Cauldron = null;
        audioSource.PlayOneShot(MetalClunk);
    }

    public void Interact()
    {
        switch (state)
        {
            case State.Active:
                ExitInteraction();
                return;
            case State.Entering:
            case State.Exiting:
                return;
        }
        if (!interactable) return;
        bool playerHasItem = PlayerController.Instance.HasItem();
        if (canPlaceCauldron)
        {
            if (playerHasItem && Cauldron != null)
            {
                PopupManager.Instance.CreatePopup("Cant interact whilst carring cauldron!");
                return;
            }
            if (playerHasItem && Cauldron == null)
                PlaceCauldron();
        }
        else if (playerHasItem)
        {
            PopupManager.Instance.CreatePopup("Cant interact whilst carring cauldron!");
            return;
        }
        EnterInteraction();
    }
    private void EnterInteraction()
    {
        state = State.Entering;
        beingInteractedWith = true;
        interactable = false;
        PlayerController.Instance.isActive = false;

        camParent = Camera.main.transform.parent;
        Camera.main.transform.SetParent(null);
        currentVelocity = Vector3.zero;
        StartCoroutine(InteractionSequence());
    }

    private void ExitInteraction()
    {
        if (camParent == null)
        {
            Debug.LogError($"{name}: camParent is null on exit", this);
        }

        state = State.Exiting;
        beingInteractedWith = false;
        OnUninteract();
        currentVelocity = Vector3.zero;
        StartCoroutine(UninteractionSequence());
    }


    public IEnumerator InteractionSequence()
    {
        audioSource.PlayOneShot(Whoosh);
        while ((Camera.main.transform.position - camTarget.position).sqrMagnitude > 0.01f)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, camTarget.position, ref currentVelocity, smoothTime);
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, camTarget.rotation, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
        state = State.Active;
        OnInteract();
    }

    public IEnumerator UninteractionSequence()
    {
        audioSource.PlayOneShot(Whoosh);
        while (camParent != null && (Camera.main.transform.position - camParent.position).sqrMagnitude > 0.01f)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, camParent.position, ref currentVelocity, smoothTime);
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, camParent.rotation, degreesPerSecond * Time.deltaTime);
            yield return null;
        }
        if (camParent != null) Camera.main.transform.SetParent(camParent);
        PlayerController.Instance.isActive = true;
        state = State.Idle;
    }

    public virtual void OnInteract()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public virtual void OnUninteract()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
