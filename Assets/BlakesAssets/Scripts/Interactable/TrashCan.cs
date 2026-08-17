using UnityEngine;
using UnityEngine.InputSystem;
public class TrashCan : MonoBehaviour
{
    public InputAction interactAction;
    public GameObject interactionPopup;
    public AudioSource source;
    private bool interactable;
    private void OnEnable()
    {
        interactAction.started += Interact;
        interactAction.Enable();
    }
    private void OnDisable()
    {
        interactAction.started -= Interact;
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
        if (context.started && !PlayerController.Instance.HasItem() && interactable)
        {
            PopupManager.Instance.CreatePopup("Nothing to bin!");
            return;
        }
        if (context.started && PlayerController.Instance.HasItem() && interactable)
        {
            PlayerController.Instance.TrashItem();
            source.Play();
        }
    }
}
