using UnityEngine.AI;
using UnityEngine;

public class NavAgentController : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveAgent", 1f, 1f);
    }

    void MoveAgent()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
