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
    public Vector3 Liquid;
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
    public GameObject NPCPrefab;
    public Transform SpawnPointPos;
    public Transform OrderCounterPos;
    public Transform WaitingLinePos;
    public Transform PickUpCounterPos;
    public Vector3 QueueSeperation;
    public List<GameObject> OrderQueue { get; private set; }
    public List<GameObject> WaitingQueue { get; private set; }

    [Header("Settings")]
    public NPCSettings currentSettings;
    [SerializeField] private List<IngredientMap> IngredientSettings = new List<IngredientMap>();
    public void Start()
    {
        OrderQueue = new List<GameObject>();
        WaitingQueue = new List<GameObject>();
        Invoke("MoveNPCInOrderQueue", 1f);
        Invoke("MoveNPCInWaitingQueue", 1f);
    }
    private OrderLayout GenerateOrderContents()
    {
        OrderLayout layout = new OrderLayout();
        List<Ingredient> contents = new List<Ingredient>();
        float timeToBrew = currentSettings.defaultTimeToBrew;
        float tempreture = currentSettings.defaultTempreture;
        layout.Liquid = randomColourValue();
        for (int i = 0; i < Mathf.Clamp(currentSettings.minIngredients + extraIngredient(), 2,3); i++)
        {
            int randomInt = UnityEngine.Random.Range(0, (int)Ingredient.Count);
            Ingredient randomIngredient = (Ingredient)randomInt;
            contents.Add(randomIngredient);
            timeToBrew += IngredientSettings.Find(item => item.ingredient == randomIngredient).addedTime;
            tempreture += IngredientSettings.Find(item => item.ingredient == randomIngredient).addedTemp;
        }
        layout.Contents = contents.ToArray();
        layout.BrewTime = timeToBrew;
        layout.Tempreture = Mathf.Clamp01(tempreture);
        return layout;
    }
    private int extraIngredient()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand <= currentSettings.chanceOfExtraIngredient*100) return 1;
        return 0;
    }
    private Vector3 randomColourValue()
    {
        List<Vector3> colors = new List<Vector3>();
        colors.Add(new Vector3(0.4047886f, 0.4956911f, 0.7683987f));
        colors.Add((colors[0] + new Vector3(1.5f, -0.5f, -0.5f)).normalized); //1r0g0b
        colors.Add((colors[1] + new Vector3(-0.5f, 1.5f, -0.5f)).normalized); //1r1g0b
        colors.Add((colors[2] + new Vector3(-0.5f, -0.5f, 1.5f)).normalized); //1r1g1b
        colors.Add((colors[1] + new Vector3(-0.5f, -0.5f, 1.5f)).normalized); //1r0g1b

        colors.Add((colors[0] + new Vector3(-0.5f, 1.5f, -0.5f)).normalized); //0r1g0b
        colors.Add((colors[5] + new Vector3(-0.5f, -0.5f, 1.5f)).normalized); //0r1g1b


        colors.Add((colors[0] + new Vector3(-0.5f, -0.5f, 1.5f)).normalized); //0r0g1b


        return colors[UnityEngine.Random.Range(0, colors.Count - 1)];
    }
    public NPCBehaviour GetFirstNPC()
    {
        GameObject NPC = OrderQueue[0];
        NPCBehaviour behaviour = NPC.GetComponent<NPCBehaviour>();
        return behaviour;
    }
    public void NPCTakeOrder()
    {
        GameObject NPC = OrderQueue[0];
        OrderQueue.Remove(NPC);
        WaitingQueue.Add(NPC);
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
    public void MoveNPCInWaitingQueue()
    {
        for (int i = 0; i < WaitingQueue.Count; i++)
        {
            GameObject NPC = WaitingQueue[i];
            Vector3 currentPos = NPC.transform.position;
            Vector3 targetPos = WaitingLinePos.position + QueueSeperation * i;
            if ((currentPos - targetPos).sqrMagnitude >= 0.5f)
            {
                NPC.GetComponent<NPCBehaviour>().goTo(targetPos);
            }
        }
        Invoke("MoveNPCInWaitingQueue", 1f);
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