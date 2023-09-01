using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;


public class FindPathAStar : MonoBehaviour {
    public Maze maze;
    public Material openMaterial;
    public Material closeMaterial;
    private List<PathMarker> _open = new List<PathMarker>();
    private List<PathMarker> _close = new List<PathMarker>();
    public GameObject start;
    public GameObject goal;
    public GameObject pathMarker;
    public PathMarker lastNode;
    private PathMarker goalNode;
    private PathMarker startNode;
    private bool isWorkEnd = false;
    public float waitTimeSecond = 0.1f;

    public bool autoSearch = false;

    const string MarkerTag = "marker";

    void RemoveAllMarker() {
        GameObject[] markers = GameObject.FindGameObjectsWithTag(MarkerTag);
        foreach (GameObject marker in markers) {
            DestroyImmediate(marker);
        }
    }

    public void BeginSearch() {
        isWorkEnd = false;
        RemoveAllMarker();

        List<MapLocation> locations = new List<MapLocation>();

        for (int _z = 0; _z < maze.depth - 1; _z++) {
            for (int _x = 0; _x < maze.width; _x++) {
                if (maze.map[_x, _z] == 0) {
                    locations.Add(new MapLocation(_x, _z));
                }
            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);

        GameObject startGameObject = Instantiate(start, startLocation, Quaternion.identity);
        GameObject goalGameObject = Instantiate(goal, goalLocation, Quaternion.identity);

        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), startGameObject, null, 0, 0, 0);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), goalGameObject, null, 0, 0, 0);

        _open.Clear();
        _close.Clear();
        _open.Add(startNode);
        _close.Add(startNode);
        lastNode = startNode;
    }

    public void StepSearch(PathMarker thisNode) {
        if (thisNode == null) return;
        if (thisNode.Equals(goalNode)) {
            isWorkEnd = true;
            ShowFinalPath(thisNode);
            return;
        }

        foreach (MapLocation dir in maze.directions) {
            MapLocation neighbor = dir + thisNode.Location;


            if (neighbor.z >= maze.depth || neighbor.z < 1)
                continue;

            if (neighbor.x >= maze.width || neighbor.x < 1)
                continue;

            // hit wall
            if (maze.map[neighbor.x, neighbor.z] == 1)
                continue;

            if (IsClosed(neighbor))
                continue;



            float G = Vector2.Distance(neighbor.ToVector(), thisNode.Location.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbor.ToVector(), startNode.Location.ToVector());
            float F = G + H;

            Vector3 neighborPosition = new Vector3(neighbor.x * maze.scale, 0, neighbor.z * maze.scale);
            GameObject pathBlock = Instantiate(pathMarker, neighborPosition, Quaternion.identity);

            UpdatePathMarkers(new PathMarker(neighbor, pathBlock, thisNode, G, H, F));
        }

        _open = _open.OrderBy(marker => marker.F).ThenBy(marker => marker.G).ToList();
        PathMarker pm = (PathMarker)_open.ElementAt(0);
        pm.Marker.GetComponent<Renderer>().material = closeMaterial;
        _close.Add(pm);
        lastNode = pm;

        _open.RemoveAt(0);
    }


    void UpdatePathMarkers(PathMarker pathMarker) {
        foreach (PathMarker openPath in _open) {
            if (openPath.Equals(pathMarker)) {
                openPath.G = pathMarker.G;
                openPath.H = pathMarker.H;
                openPath.F = pathMarker.F;
                return;
            }
        }
        _open.Add(pathMarker);
    }

    void ShowFinalPath(PathMarker finalNode) {
        if (!isWorkEnd)
            return;

        List<GameObject> pathBlocks = new List<GameObject>();
        RemoveAllMarker();
        PathMarker currentNode = finalNode;
        while (currentNode != null) {
            Vector3 position = new Vector3(currentNode.Location.x * maze.scale, 0, currentNode.Location.z * maze.scale);
            GameObject pathBlock = Instantiate(pathMarker, position, Quaternion.identity);
            pathBlock.GetComponent<Renderer>().material = openMaterial;
            pathBlocks.Add(pathBlock);
            currentNode = currentNode.Parent;
        }
        pathBlocks[0].GetComponent<Renderer>().material = start.GetComponent<Renderer>().sharedMaterial;
        pathBlocks[pathBlocks.Count - 1].GetComponent<Renderer>().material = goal.GetComponent<Renderer>().sharedMaterial;
    }

    bool IsClosed(MapLocation location) {
        foreach (PathMarker marker in _close) {
            if (marker.Location.Equals(location))
                return true;
        }
        return false;
    }

    public IEnumerator StartAutoSearch() {
        int maxDepth = 1000;
        while (lastNode != null && maxDepth > 0 && autoSearch) {
            StepSearch(lastNode);
            maxDepth--;
            yield return new WaitForSeconds(waitTimeSecond);
        }

    }
    void Start() {
        BeginSearch();
    }

}