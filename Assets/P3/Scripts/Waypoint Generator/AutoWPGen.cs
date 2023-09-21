using UnityEngine;
using System.Collections.Generic;
using MyBox;
using System.Collections;

[RequireComponent(typeof(WaypointManager))]
[RequireComponent(typeof(EdgeDebug))]
[ExecuteInEditMode]
public class AutoWPGen : MonoBehaviour
{
  [Separator("Map Settings")]
  public Vector3 mapPosition;
  public Vector3 mapSize;
  public bool showMapArea = true;
  [ConditionalField(nameof(showMapArea))] public Color areaMapColor = Color.green;
  [SerializeField] private List<ZoneData> zones = new List<ZoneData>();
  [SerializeField] private GameObject zonePrefab;
  [SerializeField][ReadOnly] private GameObject zonesContainer;

  [Separator("Waypoint Settings")]
  [SerializeField] private float ySpacing = 1f;
  [SerializeField] private float zSpacing = 1f;
  [SerializeField] private float xSpacing = 1f;
  [SerializeField] private float radius = 1f;
  [SerializeField] private bool showWaypoints = true;
  [SerializeField] private GameObject waypointPrefab;
  [SerializeField] private Transform waypointsContainer;
  private WaypointManager waypointManager;

  [Separator("Edge Settings")]
  [SerializeField] private bool showEdge = true;
  [SerializeField] private float defaultCost = 0f;
  [SerializeField] private LayerMask obstacleLayer;
  [SerializeField][ReadOnly] private const string DEFUALT_LAYER = "Ignore Raycast";


  private void OnEnable()
  {
    GetWaypointManager();
    GetZonesContainer();
  }

  public void ClearPath()
  {
    RemoveAllWaypoints();
    RemoveAllZones();
  }

  public void GenerateWaypoints()
  {
    ClearPath();
    StartCoroutine(GenerateWaypointsCoroutine());
  }

  private IEnumerator GenerateWaypointsCoroutine()
  {
    Vector3Int countNodes = CalculateNodeCount();
    GameObject[][][] nodes = new GameObject[countNodes.x][][];
    for (int x = 0; x < countNodes.x; x++)
    {
      nodes[x] = new GameObject[countNodes.y][];
      for (int y = 0; y < countNodes.y; y++)
      {
        nodes[x][y] = new GameObject[countNodes.z];
        for (int z = 0; z < countNodes.z; z++)
        {
          float xPos = -mapSize.x / 2 + x * xSpacing;
          float yPos = -mapSize.y / 2 + y * xSpacing;
          float zPos = -mapSize.z / 2 + z * xSpacing;
          Vector3 waypointPosition = mapPosition + new Vector3(xPos, yPos, zPos);
          GameObject waypoint = Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);

          if (CheckForCollisions(waypoint))
          {
            DestroyImmediate(waypoint);
          }
          else
          {
            waypointManager.waypoints.Add(waypoint);
            nodes[x][y][z] = waypointManager.waypoints[waypointManager.waypoints.Count - 1];
            waypoint.transform.SetParent(waypointsContainer);
            waypoint.name = $"WP{x}{y}{z}";
          }
        }
      }
    }

    GenerateZones();
    yield return null;

    AddEdge(nodes);
  }


  private bool CheckForCollisions(GameObject obj)
  {
    Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radius, obstacleLayer);
    return colliders.Length > 0;
  }

  private void RemoveAllWaypoints()
  {
    foreach (GameObject waypoint in waypointManager.waypoints)
    {
      DestroyImmediate(waypoint);
    }

    for (int i = waypointsContainer.childCount - 1; i >= 0; i--)
    {
      DestroyImmediate(waypointsContainer.GetChild(i).gameObject);
    }

    waypointManager.waypoints.Clear();
    waypointManager.links.Clear();
  }

  private void RemoveAllZones()
  {
    for (int i = zonesContainer.transform.childCount - 1; i >= 0; i--)
    {
      DestroyImmediate(zonesContainer.transform.GetChild(i).gameObject);
    }
  }

  private void GetWaypointManager()
  {
    if (waypointManager == null)
    {
      waypointManager = GetComponent<WaypointManager>();
    }
  }

  private void GetZonesContainer()
  {
    if (zonesContainer == null)
    {
      zonesContainer = GameObject.FindWithTag("zones");
    }
  }

  private void Update()
  {
    foreach (ZoneData zone in zones)
    {
      zone.DrawCubArea();
    }

    if (showMapArea)
    {
      MapAreaDebug.DrawCubArea(mapPosition, mapSize, areaMapColor);
    }

    UpdateVisibilityWaypoints();
  }

  public Vector3Int CalculateNodeCount()
  {
    int xNodeCount = Mathf.FloorToInt(mapSize.x / xSpacing);
    int yNodeCount = Mathf.FloorToInt(mapSize.y / ySpacing);
    int zNodeCount = Mathf.FloorToInt(mapSize.z / zSpacing);

    return new Vector3Int(xNodeCount, yNodeCount, zNodeCount);
  }



  public List<Vector3Int> GetNeighbors(Vector3Int node)
  {
    List<Vector3Int> neighbors = new List<Vector3Int>();
    Vector3Int countNodes = CalculateNodeCount();
    for (int z = -1; z <= 1; z++)
      for (int y = -1; y <= 1; y++)
        for (int x = -1; x <= 1; x++)
        {
          if (x == 0 && z == 0)
            continue;

          int neighborX = node.x + x;
          if (neighborX < 0 || neighborX >= countNodes.x)
            continue;

          int neighborY = node.y + y;
          if (neighborY < 0 || neighborY >= countNodes.y)
            continue;

          int neighborZ = node.z + z;
          if (neighborZ < 0 || neighborZ >= countNodes.z)
            continue;

          neighbors.Add(new Vector3Int(neighborX, neighborY, neighborZ));
        }
    return neighbors;
  }

  private bool CanNodesConnect(GameObject nodeA, GameObject nodeB)
  {
    Vector3 direction = nodeB.transform.position - nodeA.transform.position;
    float distance = direction.magnitude;
    LayerInfo obstacle = new LayersFilter(obstacleLayer).FirstLayer;

    RaycastHit hit;
    if (Physics.SphereCast(nodeA.transform.position, radius, direction, out hit, distance, obstacleLayer))
    {
      if (hit.collider.gameObject.layer == obstacle.Index)
      {
        // Debug.Log(hit.collider.gameObject.name);
        return false;
      }
    }
    return true;
  }

  private void AddEdge(GameObject[][][] nodes)
  {
    HashSet<string> uniqueLink = new HashSet<string>();

    for (int x = 0; x < nodes.Length; x++)
      for (int y = 0; y < nodes[x].Length; y++)
        for (int z = 0; z < nodes[x][y].Length; z++)
        {
          Vector3Int currentNodePosition = new Vector3Int(x, y, z);
          GameObject currentNode = nodes[x][y][z];

          if (currentNode == null)
            continue;

          List<Vector3Int> neighbors = GetNeighbors(currentNodePosition);

          foreach (Vector3Int neighborPosition in neighbors)
          {
            GameObject neighbor = nodes[neighborPosition.x][neighborPosition.y][neighborPosition.z];

            if (neighbor == null)
              continue;

            string link1 = $"{neighbor.name}{currentNode.name}";
            string link2 = $"{currentNode.name}{neighbor.name}";

            if (uniqueLink.Add(link1) && uniqueLink.Add(link2))
            {
              if (CanNodesConnect(currentNode, neighbor))
              {
                waypointManager.links.Add(
                  new Link(
                       neighbor,
                       currentNode,
                       Link.Direction.BI_DIRECTIONAL,
                     CalculatePathCost(currentNode, neighbor)
                     )
                );
              }
            }
          }
        }
  }


  bool _prevShowEdge = true;
  private void OnValidate()
  {
#if UNITY_EDITOR
    if (showEdge != _prevShowEdge)
    {
      GetComponent<EdgeDebug>().enabled = showEdge;
      _prevShowEdge = showEdge;
    }
#endif
  }

  bool _prevShowWaypoints = true;
  void UpdateVisibilityWaypoints()
  {
    if (showWaypoints != _prevShowWaypoints)
    {
      waypointsContainer.gameObject.SetActive(showWaypoints);
      _prevShowWaypoints = showWaypoints;
    }
  }

  void GenerateZones()
  {
    foreach (ZoneData zoneData in zones)
    {
      GameObject zone = Instantiate(zonePrefab, zoneData.position, Quaternion.identity);
      zone.transform.SetParent(zonesContainer.transform);
      zone.GetComponent<Zone>().SetZoneData(zoneData);
    }
  }
  private float CalculatePathCost(GameObject nodeA, GameObject nodeB)
  {
    Vector3 direction = nodeB.transform.position - nodeA.transform.position;
    float distance = direction.magnitude;

    foreach (ZoneData zoneData in zones)
    {
      Collider[] colliders = Physics.OverlapSphere(nodeA.transform.position, radius, zoneData.areaLayer);
      if (colliders.Length > 0)
        return zoneData.cost;

      colliders = Physics.OverlapSphere(nodeB.transform.position, radius, zoneData.areaLayer);
      if (colliders.Length > 0)
        return zoneData.cost;

      RaycastHit hit;
      if (Physics.SphereCast(nodeA.transform.position, radius, direction, out hit, distance, zoneData.areaLayer))
      {
        LayerInfo layer = new LayersFilter(zoneData.areaLayer).FirstLayer;
        if (hit.collider.gameObject.layer == layer.Index)
        {
          // Debug.Log(nodeA.name);
          return zoneData.cost;
        }
      }
    }

    return defaultCost; // You can replace defaultCost with the appropriate default value.
  }
}

