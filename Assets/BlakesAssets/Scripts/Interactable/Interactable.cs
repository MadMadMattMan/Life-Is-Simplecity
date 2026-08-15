using UnityEngine;
using System.Collections;
public class Interactable : MonoBehaviour
{
    private Camera cam;
    public GameObject interactionPopup;
    public Transform camTarget;
    [SerializeField] private float smoothTime = 0.3f;
    private Transform camParent;
    private Vector3 currentVelocity = Vector3.zero;
    private void Start()
    {
        cam = Camera.main;
    }
    public virtual void UnhoverInteraction()
    {
        interactionPopup.SetActive(false);
    }
    public virtual void HoverInteraction()
    {
        interactionPopup.SetActive(true);
    }
    public virtual void Interact()
    {
        PlayerController.Instance.isActive = false;
        camParent = cam.transform.parent;
        cam.transform.SetParent(null);
        StartCoroutine(nameof(InteractionSequence));
    }
    public virtual void Uninteract()
    {
        PlayerController.Instance.isActive = true;
        StartCoroutine(nameof(InteractionSequence));
    }
    public virtual IEnumerator InteractionSequence()
    {
        while((cam.transform.position - camTarget.position).sqrMagnitude > 0.05f)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camTarget.position, ref currentVelocity, smoothTime);
            yield return null;
        }
    }
    public virtual IEnumerator UninteractionSequence()
    {
        yield return null;
    }
}
