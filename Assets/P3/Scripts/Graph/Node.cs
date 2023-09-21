using System.Collections.Generic;
using UnityEngine;

public class Node {
    public List<Edge> edgesList = new List<Edge>();
    private GameObject _id;

    public float f, g, h;
    public Node cameFrom;
    public Node path;

    public GameObject GetId() { return _id; }

    public Node(GameObject id) {
        if (id == null) {
            Debug.LogError("Node: Null GameObject ID passed to constructor.");
            return;
        }

        _id = id;
        path = null;
    }
}
