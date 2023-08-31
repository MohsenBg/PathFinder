public class Edge {
    public Node startNode;
    public Node goalNode;

    public Edge(Node from, Node to) {
        this.startNode = from;
        this.goalNode = to;
    }
}
