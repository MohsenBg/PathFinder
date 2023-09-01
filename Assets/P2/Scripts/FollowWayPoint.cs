using System.Collections.Generic;
using UnityEngine;

public class FollowWayPoint : MonoBehaviour {
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] [Range(0.2f, 10)] private float accuracyDistance = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotation = 10f;
    [SerializeField] private float lookAhead = 10f;

    private GameObject tracker;
    private int currentWayPointIndex = 0;

    private void Start() {
        InitializeTracker();
    }

    private void InitializeTracker() {
        tracker = CreateTrackerObject();
        UpdateTrackerPositionRotation();
    }

    private GameObject CreateTrackerObject() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(obj.GetComponent<Collider>());
        obj.GetComponent<Renderer>().enabled = false;
        return obj;
    }

    private void UpdateTrackerPositionRotation() {
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    private void ProgressTracker() {
        if (waypoints.Count == 0 || Vector3.Distance(tracker.transform.position, transform.position) > lookAhead)
            return;

        Vector3 currentWayPointPosition = waypoints[currentWayPointIndex].transform.position;
        Vector2 positionWayPoint2D = new Vector2(currentWayPointPosition.x, currentWayPointPosition.z);
        Vector2 trackerPosition2D = new Vector2(tracker.transform.position.x, tracker.transform.position.z);

        if (Vector2.Distance(positionWayPoint2D, trackerPosition2D) <= accuracyDistance) {
            currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Count;
        }

        tracker.transform.LookAt(waypoints[currentWayPointIndex].transform);
        tracker.transform.Translate(0, 0, Time.deltaTime * (speed + 20));
    }

    private void Update() {
        ProgressTracker();

        if (waypoints.Count == 0 || currentWayPointIndex >= waypoints.Count)
            return;

        Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, Time.deltaTime * rotation);
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}

