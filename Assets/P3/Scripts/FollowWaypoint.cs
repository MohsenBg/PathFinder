using UnityEngine;
using System.Collections.Generic;

public class FollowWaypoint : MonoBehaviour {
    Transform goal;

    [Range(0.2f, 10)]
    public float accuracyDistance = 1;
    [Range(0.2f, 30)]
    public float speed = 5f;
    public float rotation = 10f;
    public WayPointManger wayPointManger;
    public Transform target;
    // i use it to cash potion
    private Vector3 targetPosition;
    int currentWayPointIndex = 0;
    Graph graph;
    GameObject currentNode;



    void Start() {
        graph = wayPointManger.graph;
        targetPosition = target.position;
    }


    public void GoToTarget() {
        int first = Random.Range(0, wayPointManger.waypoints.Count);
        int sec = Random.Range(0, wayPointManger.waypoints.Count);
        currentNode = wayPointManger.waypoints[FindIndexNearestNode(this.transform)];
        GameObject goalNode = wayPointManger.waypoints[FindIndexNearestNode(target)];
        graph.AStar(currentNode, goalNode);
        currentWayPointIndex = 0;
    }

    int FindIndexNearestNode(Transform transformObj) {
        List<GameObject> waypoints = wayPointManger.waypoints;
        float lowestDistance = Vector3.SqrMagnitude(waypoints[0].transform.position - transformObj.position);
        int index = 0;
        for (int i = 1; i < waypoints.Count; i++) {
            float distance = Vector3.SqrMagnitude(waypoints[i].transform.position - transformObj.position);

            if (distance < lowestDistance) {
                lowestDistance = distance;
                index = i;

            }
        }
        return index;
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GoToTarget();
            targetPosition = target.position;
        }

        if (graph.pathList.Count < 1)
            return;

        if (currentWayPointIndex >= graph.pathList.Count) {
            float dis = Vector3.SqrMagnitude(targetPosition - transform.position);
            if (Mathf.Pow(accuracyDistance, 2f) < dis)
                MoveTo(target);
            return;
        }


        currentNode = graph.pathList[currentWayPointIndex].getId();
        float distance = Vector3.SqrMagnitude(currentNode.transform.position - transform.position);
        if (Mathf.Pow(accuracyDistance, 2f) > distance) {
            currentWayPointIndex++;
            return;
        }

        MoveTo(currentNode.transform);


    }
    void MoveTo(Transform obj) {
        Quaternion lookAtTarget = Quaternion.LookRotation(obj.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtTarget, Time.deltaTime * rotation);
        // transform.LookAt(currentNode.transform);
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
