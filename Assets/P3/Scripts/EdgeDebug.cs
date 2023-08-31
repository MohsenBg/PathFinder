using UnityEngine;

[ExecuteInEditMode]
public class EdgeDebug : MonoBehaviour {

    public WayPointManger wayPointManger;
    void Update() {

#if UNITY_EDITOR
        if (wayPointManger == null)
            return;
        foreach (Link link in wayPointManger.links) {
            Debug.DrawLine(link.node1.transform.position, link.node2.transform.position, Color.black);

        }
#endif

    }
}
