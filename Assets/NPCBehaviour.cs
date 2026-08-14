using UnityEngine;
using UnityEngine.AI;
public class NPCBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        
    }
}
