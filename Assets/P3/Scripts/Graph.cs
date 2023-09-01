using System.Collections.Generic;
using UnityEngine;

public class Graph {
    private List<Edge> edges = new List<Edge>();
    private List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public void AddNode(GameObject id) {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(GameObject from, GameObject to) {
        Node nodeFrom = FindNode(from);
        Node nodeTo = FindNode(to);
        if (nodeFrom != null && nodeTo != null) {
            Edge edge = new Edge(nodeFrom, nodeTo);
            edges.Add(edge);
            nodeFrom.edgesList.Add(edge);
        } else {
            Debug.LogWarning("Failed to add edge: One or both nodes not found.");
        }
    }

    public Node FindNode(GameObject node) {
        foreach (Node _node in nodes) {
            if (_node.GetId() == node) {
                return _node;
            }
        }
        return null;
    }

    public bool AStar(GameObject _start, GameObject _goal) {
        Node start = FindNode(_start);
        Node goal = FindNode(_goal);

        if (start == null || goal == null) {
            Debug.LogWarning("AStar: Start or goal node not found.");
            return false;
        }

        if (start.GetId() == goal.GetId()) {
            Debug.LogWarning("AStar: Start and goal node same.");
            return true;
        }

        List<Node> openNodes = new List<Node>();
        List<Node> closeNodes = new List<Node>();

        start.g = 0;
        start.h = getDistanceMagnitude(start, goal);
        start.f = start.h;
        openNodes.Add(start);

        while (openNodes.Count > 0) {
            int currentNodeIndex = getLowestF(openNodes);
            Node currentNode = openNodes[currentNodeIndex];

            if (currentNode == goal) {
                generatePath(start, goal);
                return true;
            }

            openNodes.RemoveAt(currentNodeIndex);
            closeNodes.Add(currentNode);
            Node neighbor;
            float tentativeGScore;

            foreach (Edge edge in currentNode.edgesList) {
                neighbor = edge.GetGoalNode();

                if (closeNodes.Contains(neighbor))
                    continue;

                tentativeGScore = currentNode.g + getDistanceMagnitude(currentNode, neighbor);

                if (!openNodes.Contains(neighbor) || tentativeGScore < neighbor.g) {
                    neighbor.cameFrom = currentNode;
                    neighbor.g = tentativeGScore;
                    neighbor.h = getDistanceMagnitude(neighbor, goal);
                    neighbor.f = neighbor.g + neighbor.h;

                    if (!openNodes.Contains(neighbor))
                        openNodes.Add(neighbor);
                }
            }
        }
        Debug.LogWarning("AStar: Path not found.");
        return false;
    }

    private void generatePath(Node start, Node goal) {
        pathList.Clear();
        pathList.Add(goal);
        Node currentNode = goal.cameFrom;
        while (currentNode != null && currentNode != start) {
            pathList.Insert(0, currentNode);
            currentNode = currentNode.cameFrom;
        }
        pathList.Insert(0, start);
    }

    public float getDistanceMagnitude(Node from, Node to) {
        return Vector3.SqrMagnitude(from.GetId().transform.position - to.GetId().transform.position);
    }

    private int getLowestF(List<Node> _nodes) {
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
