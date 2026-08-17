using UnityEngine;

public class PickupCounter : Interactable
{
    public GameObject OrderUI;
    public GameObject OrderDrop;
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

    }
    public static PickupCounter Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
