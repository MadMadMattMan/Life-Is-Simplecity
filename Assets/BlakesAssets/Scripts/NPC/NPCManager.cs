using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
public struct OrderCompletion
{
    public Order order;
}
public struct OrderLayout
{
    public Color Liquid;
    public Ingredient[] Contents;
    public float BrewTime;
    public float Tempreture;
}
public class NPCManager : MonoBehaviour
{
    [Serializable]
    public struct IngredientMap
    {
        public Ingredient ingredient;
        public float addedTime;
        public float addedTemp;
    }
    public UnityEvent<OrderCompletion> OnOrderCompleted;
    public UnityEvent OnOrderTaken;
    public GameObject NPCPrefab;
    public Transform SpawnPointPos;
    public Transform OrderCounterPos;
    public Transform WaitingLinePos;
    public Transform PickUpCounterPos;
    public Vector3 QueueSeperation;
    [SerializeField] private List<GameObject> OrderQueue = new List<GameObject>();
    [SerializeField] private List<GameObject> WaitingQueue = new List<GameObject>();

    [Header("Settings")]
    public NPCSettings currentSettings;
    [SerializeField] private List<IngredientMap> IngredientSettings = new List<IngredientMap>();
    public void Start()
    {
        OnOrderTaken.AddListener(NPCOrderTaken);
        Invoke("MoveNPCInOrderQueue", 1f);
    }
    private OrderLayout GenerateOrderContents()
    {
        OrderLayout layout = new OrderLayout();
        List<Ingredient> contents = new List<Ingredient>();
        float timeToBrew = currentSettings.defaultTimeToBrew;
        float tempreture = currentSettings.defaultTempreture;
        layout.Liquid = new Color(randomColourValue(), randomColourValue(), randomColourValue());
        for (int i = 0; i < currentSettings.minIngredients; i++)
        {
            int randomInt = UnityEngine.Random.Range(0, (int)Ingredient.Count);
            Ingredient randomIngredient = (Ingredient)randomInt;
            contents.Add(randomIngredient);
            timeToBrew += IngredientSettings.Find(item => item.ingredient == randomIngredient).addedTime;
            tempreture += IngredientSettings.Find(item => item.ingredient == randomIngredient).addedTemp;
        }
        layout.Contents = contents.ToArray();
        layout.BrewTime = timeToBrew;
        layout.Tempreture = tempreture;
        return layout;
    }
    private int randomColourValue()
    {
        return UnityEngine.Random.Range(0, 255);
    }
    public void NPCOrderTaken()
    {
        if (OrderQueue.Count <= 0) { PopupManager.Instance.CreatePopup("Not a person in sight");  return; }
        GameObject NPC = OrderQueue[0];
        OrderQueue.Remove(NPC);
        WaitingQueue.Add(NPC);
        NPCBehaviour behaviour = NPC.GetComponent<NPCBehaviour>();
    }
    public void NPCWaitInQueue()
    {

    }
    public void AddNPC()
    {
        GameObject NPC = Instantiate(NPCPrefab, SpawnPointPos.transform.position, SpawnPointPos.transform.rotation);
        OrderQueue.Add(NPC);
        Order order = NPC.GetComponent<Order>();
        OrderLayout layout = GenerateOrderContents();
        order.InitilizeOrder(layout.Liquid,layout.Contents,layout.BrewTime,layout.Tempreture);
    }
    private void MoveNPCInOrderQueue()
    {
        for (int i = 0; i < OrderQueue.Count; i++)
        {
            GameObject NPC = OrderQueue[i];
            Vector3 currentPos = NPC.transform.position;
            Vector3 targetPos = OrderCounterPos.position + QueueSeperation * i;
            if ((currentPos-targetPos).sqrMagnitude >= 0.5f) 
            {
                NPC.GetComponent<NPCBehaviour>().goTo(targetPos);
            }
        }
        Invoke("MoveNPCInOrderQueue", 1f);
    }
    public static NPCManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}