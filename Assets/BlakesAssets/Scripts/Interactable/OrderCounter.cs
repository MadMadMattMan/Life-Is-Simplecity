using UnityEngine;

public class OrderCounter : Interactable
{
    [Header("Order Counter")]
    public NPCBehaviour currentNPC;
    private bool isInteractedWith = false;
    public bool isShowingOrder = false;
    public bool isChatting = false;
    public override void OnInteract()
    {
        base.OnInteract();
        isInteractedWith = true;
        if (NPCManager.Instance.OrderQueue.Count <= 0) { PopupManager.Instance.CreatePopup("Not a person in sight"); return; }
        Tutorial.InteractedWithOrderCounter = true;
    }
    private void Update() {
        if (isInteractedWith && !isShowingOrder) {
            if (NPCManager.Instance.OrderQueue.Count > 0) {
                currentNPC = NPCManager.Instance.GetFirstNPC();
                currentNPC.ShowOrder(this);
                isShowingOrder = true;
            }
            else {
                currentNPC = null;
                isShowingOrder = false;
            }
        }

        if (isChatting && !playerLock)
            playerLock = true;
        if (!isChatting && playerLock)
            playerLock = false;
    }

    public override void OnUninteract()
    {
        if (isChatting) return;
        base.OnUninteract();
        isInteractedWith = false;
        isShowingOrder = false;
        if (currentNPC != null) currentNPC.HideOrder();
    }
    public static OrderCounter Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
