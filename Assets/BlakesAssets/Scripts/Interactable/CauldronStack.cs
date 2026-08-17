using UnityEngine;
using UnityEngine.InputSystem;
public class CauldronStack : MonoBehaviour
{
    public GameObject CauldronPrefab;
    public InputAction interactAction;
    public GameObject interactionPopup;
    private bool interactable;
    public AudioSource audioSource;
    public AudioClip clip;
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
            GameObject prefab = Instantiate(CauldronPrefab);
            PlayerController.Instance.CarryItem(prefab);
            audioSource.PlayOneShot(clip);
        }
    }
}
