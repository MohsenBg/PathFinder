using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AutoWPGen))]
public class EditorAutoWPGen : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        AutoWPGen autoWPGen = (AutoWPGen)target;
        if (GUILayout.Button("Generate Waypoints")) {
            autoWPGen.GenerateWaypoints();
        }
    }
}
