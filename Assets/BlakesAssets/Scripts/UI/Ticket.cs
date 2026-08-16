using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Ticket : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private TicketSlot currentSlot;
    public Sprite[] ingredientSprites = new Sprite[5];
    public Order order;
    public TMP_Text OrderNumber;
    public Image Ingredient1;
    public Image Ingredient2;
    public Image Ingredient3;
    public RawImage LiquidColour;
    public RectTransform Tempreture;
    public Image TempretureImage;
    private bool DraggingEnabled = false;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void EnableDragging(bool statement)
    {
        DraggingEnabled = statement;
    }
    public void SetTicketSlot(TicketSlot slot)
    {
        currentSlot = slot;
    }
    public void OnDrag(PointerEventData eventData)
    {
        canvas = rectTransform.parent.GetComponent<Canvas>();
        if (!DraggingEnabled) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DraggingEnabled) return;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!DraggingEnabled) return;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;
        rectTransform.localScale = Vector3.one;
        currentSlot = null;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!DraggingEnabled) return;
        if (currentSlot == null)
        {
            TicketSlot closest = GetClosestSlot();
            closest.SnapTo(gameObject);
        }
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public TicketSlot GetClosestSlot()
    {
        TicketSlot[] slots = FindObjectsByType<TicketSlot>();
        TicketSlot closest = slots[0];
        float currentDistance = (rectTransform.anchoredPosition - slots[0].GetComponent<RectTransform>().anchoredPosition).sqrMagnitude;
        foreach (TicketSlot slot in slots)
        {
            if (slot == slots[0] || slot.currentTicket != null) continue;
            float distance = (rectTransform.anchoredPosition - slot.GetComponent<RectTransform>().anchoredPosition).sqrMagnitude;
            if (distance < currentDistance)
            {
                currentDistance = distance;
                closest = slot;
            }
        }
        return closest;
    }
    public void TakeOrder(float upTime, Order order)
    {
        this.order = order;
        StartCoroutine(PrintOrder(upTime));
    }
    IEnumerator PrintOrder(float upTime)
    {
        OrderNumber.text = "Order " + order.OrderNumber;
        yield return new WaitForSeconds(upTime/order.numberOfItems);
        LiquidColour.enabled = true;
        LiquidColour.color = order.Liquid;
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        Ingredient3.enabled = true;
        Ingredient3.sprite = ingredientSprites[(int)order.Contents[0]];
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        Ingredient2.enabled = true;
        Ingredient2.sprite = ingredientSprites[(int)order.Contents[1]];
        if (order.Contents.Length > 2)
        {
            yield return new WaitForSeconds(upTime / order.numberOfItems);
            Ingredient1.enabled = true;
            Ingredient1.sprite = ingredientSprites[(int)order.Contents[2]];
        }
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        TempretureImage.enabled = true;
        Tempreture.anchoredPosition = new Vector2(Tempreture.anchoredPosition.x, Mathf.Lerp(-63f, 12f, order.Tempreture/100));
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        TicketSlot closest = GetClosestSlot();
        closest.SnapTo(gameObject);
        EnableDragging(true);
    }
}
