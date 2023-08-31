using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Link {
    public enum Direction { SINGLE, MULTI };
    public GameObject node1;
    public GameObject node2;
    public Direction direction;

}
public class WayPointManger : MonoBehaviour {

    public List<GameObject> waypoints;
    public List<Link> links;
    // Start is called before the first frame update
    public Graph graph = new Graph();
    void Start() {

        HashSet<string> uniqueWaypoint = new HashSet<string>();
        foreach (GameObject waypoint in waypoints) {
            if (uniqueWaypoint.Contains(waypoint.name))
                continue;
            graph.AddNode(waypoint);
            uniqueWaypoint.Add(waypoint.name);
        }


        HashSet<string> uniqueLink = new HashSet<string>();
        foreach (Link link in links) {
            string linkName = link.node1.name + link.node2.name;
            if (!uniqueLink.Contains(linkName)) {
                graph.AddEdge(link.node1, link.node2);
                uniqueLink.Add(linkName);
            }

            if (link.direction == Link.Direction.SINGLE)
                continue;

            linkName = link.node2.name + link.node1.name;
            if (!uniqueLink.Contains(linkName)) {
                graph.AddEdge(link.node1, link.node2);
                uniqueLink.Add(linkName);
            }
            graph.AddEdge(link.node2, link.node1);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
