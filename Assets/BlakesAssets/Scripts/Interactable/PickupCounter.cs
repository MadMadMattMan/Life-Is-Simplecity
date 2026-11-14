using UnityEngine;
using System.Collections;
public class PickupCounter : Interactable
{
    public GameObject OrderUI;
    public StarManager StarManager;
    public GameObject ticketSlot;
    public override void OnInteract()
    {
        base.OnInteract();
        OrderUI.SetActive(true);
        if (Cauldron) 
        {
            ticketSlot.SetActive(true); 
        }
    }
    public override void OnUninteract()
    {
        base.OnUninteract();
        OrderUI.SetActive(false);
        ticketSlot.SetActive(false);
    }
    public void OnOrderComplete(GameObject ticket)
    {
        StarManager.CauldronOrderToStars(Cauldron.GetComponent<CauldronManager>(),
                                             ticket.GetComponent<Ticket>().order);
        Ticket ticket1 = ticket.GetComponent<Ticket>();
        NPCManager.Instance.NPCGoOutDoor(ticket1.NPC.gameObject);
        NPCManager.Instance.NPCCollectOrder(ticket1.NPC.gameObject);
        GameManager.Instance.activeOrders.Remove(ticket1.order);
        Destroy(Cauldron, 2f);
        Destroy(ticket, 2f);
    }
    public static PickupCounter Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
