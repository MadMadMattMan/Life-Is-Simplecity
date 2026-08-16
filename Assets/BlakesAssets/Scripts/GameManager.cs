using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Order> activeOrders = new List<Order>();
    public GameObject ticketPrefab;
    [SerializeField] private NPCSettings[] SettingsList;
    [SerializeField] private NPCSettings CurrentSettings;
    [Range(0, 5)] public float StarRating;
    public GameObject[] Stars;
    public void Start()
    {
        Invoke("SpawnNPC", 5f);
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < Mathf.CeilToInt(StarRating); i++)
        {
            if (i == Mathf.CeilToInt(StarRating))
            {
                Stars[i].GetComponent<Image>().fillAmount = StarRating - Mathf.Floor(StarRating);
                return;
            }
            Stars[i].GetComponent<Image>().fillAmount = 1;
        }
    }
    public void WriteTicket(float upTime, Order order)
    {
        GameObject ticket = Instantiate(ticketPrefab, ticketPrefab.transform.position, ticketPrefab.transform.rotation);
        ticket.transform.SetParent(transform.GetChild(0));
        ticket.GetComponent<Ticket>().TakeOrder(upTime, order);
    }
    public void SpawnNPC()
    {
        if (CurrentSettings != NPCManager.Instance.currentSettings) 
        {
            NPCManager.Instance.currentSettings = CurrentSettings;
        }
        NPCManager.Instance.AddNPC();
        float randomSpawnOffset = Random.Range(-CurrentSettings.spawnTimerOffset, CurrentSettings.spawnTimerOffset);
        Invoke("SpawnNPC", CurrentSettings.defaultSpawnTimer + randomSpawnOffset);
    }
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
