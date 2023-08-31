using UnityEngine;
using System.Collections.Generic;
using System.IO;
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

    public Graph graph = new Graph();
    void Start() {
        // WriteOnText();
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

    // for Debugging
    void WriteOnText() {
        // Path to the file you want to create or overwrite
        string filePath = Application.dataPath + "/MyTextFile.txt";

        // Text content to write to the file
        string textContent = "";
        foreach (Link link in links) {
            textContent += $"{link.node1.name} -> {link.node2.name}\n";
        }

        // Write the text content to the file
        File.WriteAllText(filePath, textContent);
        Debug.Log("Text file written at: " + filePath);
    }
}
