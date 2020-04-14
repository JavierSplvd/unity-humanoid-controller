using UnityEngine.AI;
using UnityEngine;

public class NavAgentController : MonoBehaviour
{
    public GameObject target;
    public GameObject subject;
    public float maxDistanceFromSubject;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveAgent", 1f, 1f);
    }

    void MoveAgent()
    {
        if(Vector3.Distance(transform.position, subject.transform.position) < maxDistanceFromSubject)
        {
            navMeshAgent.SetDestination(target.transform.position);
            navMeshAgent.Resume();
        }
        else
        {
            navMeshAgent.Stop();
        }
    }
}
