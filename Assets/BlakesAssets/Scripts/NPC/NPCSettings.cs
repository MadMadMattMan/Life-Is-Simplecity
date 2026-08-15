using UnityEngine;

[CreateAssetMenu(fileName = "NPCSettings", menuName = "Scriptable Objects/NPCSettings")]
//Different scriptable objects for different star ratings of the shop
public class NPCSettings : ScriptableObject
{
    [Range(0,50)] public float defaultTimeToBrew = 20f;
    [Range(0,80)] public float defaultTempreture = 50f;
    [Range(1, 4)] public int minIngredients = 2;
    [Range(0, 1)] public float tolerance = 0.5f;
    [Range(5, 15)] public float defaultSpawnTimer = 10f;
    [Range(0, 7)] public float spawnTimerOffset = 3f;
}
