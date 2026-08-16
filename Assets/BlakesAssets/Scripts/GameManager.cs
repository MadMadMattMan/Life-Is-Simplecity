using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Order> activeOrders = new List<Order>();
    public GameObject ticketPrefab;
    [SerializeField] private NPCSettings[] SettingsList;
    [SerializeField] private NPCSettings CurrentSettings;
    
    public void Start()
    {
        Invoke("SpawnNPC", 5f);
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
