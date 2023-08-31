using System.Collections.Generic;
using UnityEngine;
public class Node {
    public List<Edge> edgesList = new List<Edge>();
    public Node path = null;
    private GameObject _id;

    public float f, g, h;
    public Node cameFrom;

    public GameObject getId() { return _id; }

    public Node(GameObject i) {
        _id = i;
        path = null;
    }

}
