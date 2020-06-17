using UnityEngine.AI;
using UnityEngine;

public class NavAgentController : MonoBehaviour
{
    public GameObject target;
    public GameObject subject;
    public float maxDistanceFromSubject;
    public float currentDistanceFromSubject;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = subject.transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        // InvokeRepeating("MoveAgent", 1f, 1f);
        // InvokeRepeating("Unstuck", 15f, 15f);
    }

    void Update()
    {
        MoveAgent();
    }

    void MoveAgent()
    {
        currentDistanceFromSubject = Vector3.Distance(transform.position, subject.transform.position);
        if(currentDistanceFromSubject < maxDistanceFromSubject)
        {
            navMeshAgent.SetDestination(target.transform.position);
            navMeshAgent.Resume();
        }
        else
        {
            navMeshAgent.Stop();
        }
    }

    void Unstuck()
    {
        navMeshAgent.transform.position = subject.transform.position;
    }
}
