using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class WaypointManager : MonoBehaviour {
    public List<GameObject> waypoints;
    public List<Link> links;
    public Graph graph = new Graph();

    private void Start() {
        InitializeGraph();
        // WriteOnText()
    }

    private void InitializeGraph() {
        HashSet<string> uniqueWaypoint = new HashSet<string>();
        foreach (GameObject waypoint in waypoints) {
            if (uniqueWaypoint.Add(waypoint.name)) {
                graph.AddNode(waypoint);
            }
        }

        HashSet<string> uniqueLink = new HashSet<string>();
        foreach (Link link in links) {
            string linkName1 = link.node1.name + link.node2.name;
            string linkName2 = link.node2.name + link.node1.name;

            if (uniqueLink.Add(linkName1) || link.direction == Link.Direction.BI_DIRECTIONAL) {
                graph.AddEdge(link.node1, link.node2);
            }

            if (uniqueLink.Add(linkName2) && link.direction == Link.Direction.BI_DIRECTIONAL) {
                graph.AddEdge(link.node2, link.node1);
            }
        }
    }

    // For Debugging: Writes link information to a text file
    private void WriteOnText() {
        try {
            string filePath = Application.dataPath + "/MyTextFile.txt";
            string textContent = string.Join("\n", links.Select(link => $"{link.node1.name} -> {link.node2.name}"));

            File.WriteAllText(filePath, textContent);
            Debug.Log("Text file written at: " + filePath);
        } catch (Exception e) {
            Debug.LogError("Error writing to text file: " + e.Message);
        }
    }
}
