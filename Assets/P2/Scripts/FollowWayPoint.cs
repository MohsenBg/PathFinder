using System.Collections.Generic;
using UnityEngine;

public class FollowWayPoint : MonoBehaviour {
    public List<GameObject> waypoints;
    int currentWayPointIndex = 0;

    [Range(0.2f, 10)]
    public float accuracyDistance = 1;
    public float speed = 10f;
    public float rotation = 10f;

    public float lookAhead = 10f;

    GameObject tracker;
    // Start is called before the first frame update
    void Start() {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<Renderer>().enabled = false;
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;
    }

    void ProgressTracker() {
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead)
            return;

        Vector3 currentWayPointPosition = waypoints[currentWayPointIndex].transform.position;
        Vector2 potionWayPoint2D = new Vector2(currentWayPointPosition.x, currentWayPointPosition.z);
        Vector3 trackerPosition = tracker.transform.position;
        Vector2 trackerPosition2D = new Vector2(trackerPosition.x, trackerPosition.z);
        if (Vector2.Distance(potionWayPoint2D, trackerPosition2D) <= accuracyDistance) {
            currentWayPointIndex += 1;
            if (currentWayPointIndex >= waypoints.Count)
                currentWayPointIndex = 0;
        }

        tracker.transform.LookAt(waypoints[currentWayPointIndex].transform);
        tracker.transform.Translate(0, 0, Time.deltaTime * (speed + 20));
    }
    // Update is called once per frame
    void Update() {
        ProgressTracker();

        Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, Time.deltaTime * rotation);
        //transform.LookAt(waypoints[currentWayPointIndex].transform);
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
