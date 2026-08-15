using UnityEngine;
using UnityEngine.AI;
public class NPCBehaviour : MonoBehaviour
{
    public Vector3 lookDirection;
    public float rotationSpeed = 5.0f;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
    public void goTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
