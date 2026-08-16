using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class NPCBehaviour : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private EventSystem eventSystem;
    public Animator anim;
    public GameObject Hats;
    public GameObject Belts;
    public GameObject Badges;
    public GameObject Scarfs;
    public Sprite[] IngredientSprites = new Sprite[5];
    public Sprite ThermometerIcon;
    public Sprite white;
    public GameObject speechBubbleCanvas;
    public Image speechBubble;
    public Vector3 lookDirection;
    public float rotationSpeed = 5.0f;
    public Button button;
    private NavMeshAgent agent;
    private Order order;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        order = GetComponent<Order>();
        PickRandomOutfit();
        PickRandomAnimationSpeed();
    }
    private void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
        }
    }
    private void PickRandomOutfit()
    {
        bool hasHat = Mathf.RoundToInt(Random.Range(0f, 1f)) != 0;
        bool hasBelt = Mathf.RoundToInt(Random.Range(0f, 1f)) != 0;
        bool hasBadge = Mathf.RoundToInt(Random.Range(0f, 1f)) != 0;
        bool hasScarfs = Mathf.RoundToInt(Random.Range(0f, 1f)) != 0;
        if (hasHat) Hats.transform.GetChild(Random.Range(0, Hats.transform.childCount)).gameObject.SetActive(true);
        if (hasBelt) Belts.transform.GetChild(Random.Range(0, Belts.transform.childCount)).gameObject.SetActive(true);
        if (hasBadge) Badges.transform.GetChild(Random.Range(0, Badges.transform.childCount)).gameObject.SetActive(true);
        if (hasScarfs) Scarfs.transform.GetChild(Random.Range(0, Scarfs.transform.childCount)).gameObject.SetActive(true);
    }
    private void PickRandomAnimationSpeed()
    {
        anim.speed = Random.Range(0.85f, 1.15f);
    }
    public void goTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
    public void ShowOrder()
    {
        speechBubbleCanvas.SetActive(true);
    }
    public void DisplayOrder()
    {
        button.interactable = false;
        float upTime = 5f;
        StartCoroutine(PrintOrder(upTime, order));
        GameManager.Instance.WriteTicket(upTime, order);
    }
    IEnumerator PrintOrder(float upTime, Order order)
    {
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        speechBubble.sprite = white;
        speechBubble.color = order.Liquid;
        for (int i = 0; i < order.Contents.Length; i++)
        {
            yield return new WaitForSeconds(upTime / order.numberOfItems);
            speechBubble.color = Color.white;
            speechBubble.sprite = IngredientSprites[(int)order.Contents[i]];
        }
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        speechBubble.sprite = ThermometerIcon;
        yield return new WaitForSeconds(upTime / order.numberOfItems);
        HideOrder();
        OrderCounter.Instance.isShowingOrder = false;
        OrderCounter.Instance.playerLock = false;
        NPCManager.Instance.NPCTakeOrder();
    }
    public void HideOrder()
    {
        speechBubbleCanvas.SetActive(false);
    }
}
