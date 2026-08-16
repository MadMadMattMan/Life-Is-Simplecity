using UnityEngine;

public class OrderCounter : Interactable
{
    public GameObject OrderUI;
    public NPCBehaviour currentNPC;
    private bool isInteractedWith = false;
    private bool isShowingOrder = false;
    public override void OnInteract()
    {
        base.OnInteract();
        isInteractedWith = true;
        OrderUI.SetActive(true);
        if (NPCManager.Instance.OrderQueue.Count <= 0) { PopupManager.Instance.CreatePopup("Not a person in sight"); return; }
    }
    private void Update()
    {
        if (isInteractedWith && !isShowingOrder)
        {
            if (NPCManager.Instance.OrderQueue.Count > 0)
            {
                currentNPC = NPCManager.Instance.GetFirstNPC();
                currentNPC.ShowOrder();
                isShowingOrder = true;
            }
            else
            {
                currentNPC = null;
                isShowingOrder = false;
            }
        }
    }
    public override void OnUninteract()
    {
        base.OnUninteract();
        isInteractedWith = false;
        isShowingOrder = false;
        OrderUI.SetActive(false);
        if (currentNPC!=null) currentNPC.HideOrder();
    }
}
