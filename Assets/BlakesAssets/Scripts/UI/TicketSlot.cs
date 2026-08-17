using UnityEngine;
using UnityEngine.EventSystems;

public class TicketSlot : MonoBehaviour, IDropHandler
{
    public Vector3 TicketScale;
    public GameObject currentTicket;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            currentTicket = eventData.pointerDrag;
            RectTransform rectTransform = currentTicket.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            rectTransform.localScale = TicketScale;
            currentTicket.GetComponent<Ticket>().SetTicketSlot(this);
            CompleteOrder();
        }
    }
    public void SnapTo(GameObject ticket)
    {
        currentTicket = ticket;
        RectTransform rectTransform = currentTicket.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        rectTransform.localScale = TicketScale;
        currentTicket.GetComponent<Ticket>().SetTicketSlot(this);
        CompleteOrder();
    }
    public void CompleteOrder()
    {
        if (gameObject.name == "ItemSlotComplete")
        {
            PickupCounter.Instance.OnOrderComplete(currentTicket);
        }
    }
}
