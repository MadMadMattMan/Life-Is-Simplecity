using UnityEngine;

public class PickupCounter : Interactable
{
    public GameObject OrderUI;
    public override void OnInteract()
    {
        base.OnInteract();
        OrderUI.SetActive(true);
    }
    public override void OnUninteract()
    {
        base.OnUninteract();
        OrderUI.SetActive(false);
    }
}
