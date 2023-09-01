using UnityEngine;
using System.Collections.Generic;

public class FollowWaypoint : MonoBehaviour {
    [SerializeField] [Range(0.2f, 10)] private float accuracyDistance = 1;
    [SerializeField] [Range(0.2f, 30)] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    public WaypointManager waypointManager;
    public Transform target;

    private Vector3 _targetPosition;
    private int _currentWaypointIdx = 0;
    private Graph _graph;

    private void Start() {
        _graph = waypointManager.graph;
        _targetPosition = target.position;
    }

    private void Update() {
        HandleMovementInput();

        if (_graph.pathList.Count < 1)
            return;

        if (_currentWaypointIdx >= _graph.pathList.Count) {
            HandleTargetReached();
            return;
        }

        GameObject currentNode = _graph.pathList[_currentWaypointIdx].GetId();
        float distanceToWaypoint = Vector3.SqrMagnitude(currentNode.transform.position - transform.position);

        if (distanceToWaypoint <= accuracyDistance * accuracyDistance) {
            _currentWaypointIdx++;
        } else {
            MoveTo(currentNode.transform);
        }
    }

    private void HandleMovementInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GoToTarget();
            _targetPosition = target.position;
        }
    }

    private void HandleTargetReached() {
        float distanceToTarget = Vector3.SqrMagnitude(_targetPosition - transform.position);

        if (distanceToTarget > accuracyDistance * accuracyDistance) {
            MoveTo(target);
        }
    }

    private void MoveTo(Transform obj) {
        Quaternion lookAtTarget = Quaternion.LookRotation(obj.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtTarget, Time.deltaTime * rotationSpeed);
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void GoToTarget() {
        GameObject startWaypoint = FindNearestWaypoint(transform);
        GameObject goalWaypoint = FindNearestWaypoint(target);
        if (startWaypoint == goalWaypoint)
            return;
        _graph.AStar(startWaypoint, goalWaypoint);
        _currentWaypointIdx = 0;
    }

    private GameObject FindNearestWaypoint(Transform transformObj) {
        List<GameObject> waypoints = waypointManager.waypoints;

        if (waypoints.Count == 0) {
            Debug.LogWarning("No waypoints found.");
            return null;
        }

        GameObject nearestWaypoint = waypoints[0];
        float shortestDistance = Vector3.SqrMagnitude(nearestWaypoint.transform.position - transformObj.position);

        for (int i = 1; i < waypoints.Count; i++) {
            GameObject currentWaypoint = waypoints[i];
            float distance = Vector3.SqrMagnitude(currentWaypoint.transform.position - transformObj.position);

            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestWaypoint = currentWaypoint;
            }
        }

        return nearestWaypoint;
    }
}
