using UnityEngine;


[System.Serializable]
public struct Link
{
  public enum Direction { SINGLE_DIRECTIONAL, BI_DIRECTIONAL };
  public GameObject node1;
  public GameObject node2;
  public Direction direction;
  public float cost;
  public Link(GameObject n1, GameObject n2, Direction dir, float cost = 0f)
  {
    if (n1 == null || n2 == null)
    {
      throw new System.ArgumentNullException("Node references cannot be null.");
    }
    node1 = n1;
    node2 = n2;
    direction = dir;
    this.cost = cost;
  }
}
