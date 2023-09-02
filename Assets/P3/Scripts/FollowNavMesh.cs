using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowNavMesh : MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] [Range(0.2f, 10)] private float accuracyDistance = 1;
    [SerializeField] [Range(0.2f, 30)] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start() {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        UpdateNavMesh();
    }

    void UpdateNavMesh() {
        if (navMeshAgent == null)
            return;




        if (navMeshAgent.destination != target.transform.position)
            navMeshAgent.SetDestination(target.position);

        if (navMeshAgent.stoppingDistance != accuracyDistance)
            navMeshAgent.stoppingDistance = accuracyDistance;

        if (navMeshAgent.speed != speed)
            navMeshAgent.speed = speed;

        if (navMeshAgent.angularSpeed != rotationSpeed)
            navMeshAgent.angularSpeed = rotationSpeed;


    }
}
