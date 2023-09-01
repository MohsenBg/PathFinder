using UnityEngine;

[ExecuteInEditMode]
public class EdgeDebug : MonoBehaviour {
    [SerializeField]
    private WaypointManager waypointManager;

    private void OnValidate() {
        if (waypointManager == null) {
            Debug.LogWarning("EdgeDebug: WayPointManager is not assigned.");
        }
    }

    private void Update() {
#if UNITY_EDITOR
        if (waypointManager == null) {
            Debug.LogWarning("EdgeDebug: WayPointManager is not assigned.");
            return;
        }

        foreach (Link link in waypointManager.links) {
            if (link.node1 == null || link.node2 == null) {
                Debug.LogWarning("EdgeDebug: Invalid link or nodes in WaypointManager.");
                continue;
            }

            Debug.DrawLine(link.node1.transform.position, link.node2.transform.position, Color.black);
        }
#endif
    }
}
