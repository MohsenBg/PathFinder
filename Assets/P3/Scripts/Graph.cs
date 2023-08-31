using System.Collections.Generic;
using UnityEngine;
public class Graph {
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph() { }

    public void AddNode(GameObject id) {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(GameObject from, GameObject to) {
        Node nodeFrom = FindNode(from);
        Node nodeTo = FindNode(to);
        if (nodeFrom == null || nodeTo == null)
            return;
        Edge edge = new Edge(nodeFrom, nodeTo);
        edges.Add(edge);
        nodeFrom.edgesList.Add(edge);
    }

    public Node FindNode(GameObject node) {
        foreach (Node _node in nodes) {
            if (_node.getId() == node.gameObject) {
                return _node;
            }
        }
        return null;
    }


    public bool AStar(GameObject _start, GameObject _goal) {
        pathList.Clear();
        Node start = FindNode(_start);
        Node goal = FindNode(_goal);
        if (start == null || goal == null)
            return false;

        start.g = 0;
        start.h = getDistanceMagnitude(start, goal);
        start.f = start.h;
        List<Node> openNodes = new List<Node>();
        List<Node> closeNodes = new List<Node>();
        openNodes.Add(start);


        int maxDepth = 100;
        float gScore = 0;
        while (openNodes.Count > 0 && maxDepth > 0) {
            maxDepth--;
            int currentNodeIndex = getLowestF(openNodes);
            Node currentNode = openNodes[currentNodeIndex];
            if (currentNode.getId() == goal.getId()) {
                generatePath(currentNode);
                return true;
            }
            openNodes.RemoveAt(currentNodeIndex);
            closeNodes.Add(currentNode);
            Node neighbor;
            foreach (Edge edge in currentNode.edgesList) {
                neighbor = edge.goalNode;

                if (closeNodes.IndexOf(neighbor) > -1)
                    continue;


                gScore = currentNode.g + getDistanceMagnitude(currentNode, neighbor);
                if (openNodes.IndexOf(neighbor) == -1) {
                    neighbor.g = gScore;
                    openNodes.Add(neighbor);
                }
                if (neighbor.g >= gScore) {
                    neighbor.g = gScore;
                    neighbor.h = getDistanceMagnitude(neighbor, goal);
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.cameFrom = currentNode;
                }
            }
        }
        return false;

    }

    private void generatePath(Node finalNode) {
        Node currentNode = finalNode;
        while (currentNode != null) {
            pathList.Insert(0, currentNode);
            currentNode = currentNode.cameFrom;
        }
    }

    public float getDistanceMagnitude(Node from, Node to) {
        // it way more faster to calculate sqrMagnitude than distance it self  
        return Vector3.SqrMagnitude(from.getId().transform.position - to.getId().transform.position);
    }

    int getLowestF(List<Node> _nodes) {
        float lowestF = _nodes[0].f;
        int index = 0;
        for (int i = 1; i < _nodes.Count; i++) {
            if (_nodes[i].f < lowestF) {
                lowestF = _nodes[i].f;
                index = i;
            }
        }
        return index;
    }
}
