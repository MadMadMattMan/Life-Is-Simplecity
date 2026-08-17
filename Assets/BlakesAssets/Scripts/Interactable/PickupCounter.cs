using UnityEngine;

public class PickupCounter : Interactable
{
    public GameObject OrderUI;
    public GameObject OrderDrop;
    public StarManager StarManager;
    public override void OnInteract()
    {
        base.OnInteract();
        OrderDrop.SetActive(true);
        OrderUI.SetActive(true);
    }
    public override void OnUninteract()
    {
        base.OnUninteract();
        OrderDrop.SetActive(false);
        OrderUI.SetActive(false);
    }
    public void OnOrderComplete(GameObject ticket)
    {
        if (Cauldron)

        StarManager.CauldronOrderToStars(Cauldron.GetComponentInChildren<CauldronManager>(),
                                         ticket.GetComponent<Ticket>().order);

            // trigger customer leave
    }
    public static PickupCounter Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
