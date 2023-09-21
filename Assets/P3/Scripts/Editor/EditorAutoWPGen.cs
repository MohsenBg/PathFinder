using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AutoWPGen))]
public class EditorAutoWPGen : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    AutoWPGen autoWPGen = (AutoWPGen)target;

    GUILayout.Space(10);

    if (GUILayout.Button("Generate Path"))
    {
      autoWPGen.GenerateWaypoints();
    }

    GUILayout.Space(5);

    if (GUILayout.Button("Clear Path"))
    {
      autoWPGen.ClearPath();
    }
  }
}
