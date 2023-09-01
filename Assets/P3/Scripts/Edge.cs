public class Edge {
    private Node startNode;
    private Node goalNode;

    public Edge(Node from, Node to) {
        if (from == null || to == null) {
            throw new System.ArgumentNullException("Nodes in an edge cannot be null.");
        }

        this.startNode = from;
        this.goalNode = to;
    }

    public Node GetStartNode() {
        return startNode;
    }

    public Node GetGoalNode() {
        return goalNode;
    }
}

