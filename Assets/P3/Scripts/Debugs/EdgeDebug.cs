using UnityEngine;
using MyBox;
using System.Collections.Generic;

[System.Serializable]
struct EdgeColor
{
  public float minCost;
  public float maxCost;
  public Color color;
}

[ExecuteInEditMode]
public class EdgeDebug : MonoBehaviour
{
  [SerializeField] private Color defaultEdgeColor = Color.black;
  [SerializeField] private WaypointManager waypointManager;
  [SerializeField] private List<EdgeColor> edgeColors = new List<EdgeColor>();

  private void OnValidate()
  {
    if (waypointManager == null)
    {
      Debug.LogWarning("EdgeDebug: WayPointManager is not assigned.");
    }
  }

  private void Update()
  {
#if UNITY_EDITOR
    if (waypointManager == null)
    {
      Debug.LogWarning("EdgeDebug: WayPointManager is not assigned.");
      return;
    }

    foreach (Link link in waypointManager.links)
    {
      if (link.node1 == null || link.node2 == null)
      {
        Debug.LogWarning("EdgeDebug: Invalid link or nodes in WaypointManager.");
        continue;
      }

      Debug.DrawLine(link.node1.transform.position, link.node2.transform.position, GetColor(link.cost));
    }
#endif
  }


  Color GetColor(float cost)
  {
    foreach (EdgeColor edgeColor in edgeColors)
    {
      if (cost >= edgeColor.minCost && cost <= edgeColor.maxCost)
      {
        return edgeColor.color;
      }
    }
    return defaultEdgeColor;
  }
}
