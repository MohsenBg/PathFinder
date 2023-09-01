using UnityEngine;

[ExecuteInEditMode]
public class WaypointDebug : MonoBehaviour {
    void RenameWPs(GameObject overlook) {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("wp");
        int i = 1;
        foreach (GameObject go in gos) {
            if (go != overlook) {
                go.name = "WP" + string.Format("{0:000}", i);
                i++;
            }
        }
    }

    void OnDestroy() {
        RenameWPs(this.gameObject);
    }

    // Use this for initialization
    void Start() {
        if (this.transform.parent == null) {
            Debug.LogWarning("WaypointDebug: Parent transform is null.");
            return;
        }

        if (this.transform.parent.gameObject.name != "WayPoint") {
            RenameWPs(null);
        }
    }

    // Update is called once per frame
    void Update() {
        TextMesh textMesh = this.GetComponent<TextMesh>();
        if (textMesh != null) {
            textMesh.text = this.transform.parent.gameObject.name;
        } else {
            Debug.LogWarning("WaypointDebug: TextMesh component not found.");
        }
    }
}
