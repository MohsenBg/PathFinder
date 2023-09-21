public class Edge
{
  private Node startNode;
  private Node goalNode;
  private float cost = 0f;
  public Edge(Node from, Node to, float cost = 0f)
  {
    if (from == null || to == null)
    {
      throw new System.ArgumentNullException("Nodes in an edge cannot be null.");
    }

    this.startNode = from;
    this.goalNode = to;
    this.cost = cost;
  }

  public Node GetStartNode()
  {
    return startNode;
  }

  public Node GetGoalNode()
  {
    return goalNode;
  }

  public float GeCost()
  {
    return cost;
  }
}

