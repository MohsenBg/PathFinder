using UnityEngine;
using System.Collections.Generic;

public class AutoWPGen : MonoBehaviour {
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size;
    [SerializeField] private float radios = 10;
    [SerializeField] private float distanceCol = 1;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private int col = 3;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private Transform waypointsContainer;
    [SerializeField] GameObject waypointPrefab;
    [SerializeField] WaypointManager waypointManager;
    [SerializeField] LayerMask obstacleLayer;

    public void GenerateWaypoints() {
        RemoveAllWaypoints();
        int currentWp = 1;

        float _distanceCol = distanceCol;
        for (int _col = 0; _col < col; _col++) {
            for (float x = -size.x / 2; x < size.x / 2; x += spacing) {
                for (float z = -size.z / 2; z < size.z / 2; z += spacing) {
                    Vector3 waypointPosition = center + new Vector3(x, _col * distanceCol, z);
                    GameObject waypoint =
                    Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);
                    if (CheckForCollisions(waypoint)) {
                        DestroyImmediate(waypoint);
                        continue;
                    }
                    waypoint.name = "WP" + currentWp.ToString("000");
                    waypoint.transform.SetParent(waypointsContainer.transform);
                    waypointManager.waypoints.Add(waypoint);
                    currentWp++;
                }
            }
        }
        AddEdges();
    }

    private void AddEdges() {
        waypointManager.links.Clear();
        List<GameObject> waypoints = waypointManager.waypoints;
        for (int i = 0; i < waypoints.Count; i++) {
            for (int j = i; j < waypoints.Count; j++) {
                if (waypoints[i] == waypoints[j])
                    continue;

                Vector3 posA = waypoints[i].transform.position;
                Vector3 posB = waypoints[j].transform.position;

                if (posA.y != posB.y && posA.x == posB.x && posA.z == posB.z)
                    continue;

                if (CanNodesConnect(waypoints[i], waypoints[j])) {
                    // Debug.Log(waypoints[i].name + "->" + waypoints[j].name); 
                    waypointManager.links.Add(new Link(waypoints[i], waypoints[j], Link.Direction.BI_DIRECTIONAL));
                }
            }
        }
    }

    private bool CheckForCollisions(GameObject obj) {
        Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radios);
        return colliders.Length > 0;
    }

    private void RemoveAllWaypoints() {
        foreach (GameObject waypoint in waypointManager.waypoints) {
            if (waypoint != null)
                DestroyImmediate(waypoint);
        }
        Transform[] children = new Transform[waypointsContainer.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }

        foreach (Transform child in children) {
            if (child != null)
                DestroyImmediate(child.gameObject);
        }

        waypointManager.waypoints.Clear();
    }

    private bool CanNodesConnect(GameObject nodeA, GameObject nodeB) {
        Vector3 direction = nodeB.transform.position - nodeA.transform.position;
        float distance = direction.magnitude;

        if (distance > maxDistance)
            return false;

        RaycastHit hit;
        if (Physics.SphereCast(nodeA.transform.position, radios, direction, out hit, distance, obstacleLayer)) {
            return false;
        }

        return true;
    }
}