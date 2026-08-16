using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private NPCSettings[] SettingsList;
    [SerializeField] private NPCSettings CurrentSettings;
    [Range(0, 5)] public float StarRating;
    public void Start()
    {
        Invoke("SpawnNPC", 5f);
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
