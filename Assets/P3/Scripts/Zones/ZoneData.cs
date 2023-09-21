using UnityEngine;
using MyBox;

[System.Serializable]
public class ZoneData
{
  public string name;
  public Vector3 position;
  public Vector3 size;
  public float cost;
  public bool showArea = false;
  [ConditionalField(nameof(showArea))]
  public Color color = Color.black;
  public LayerMask areaLayer;
  public void DrawCubArea()
  {
    if (!showArea)
      return;
    MapAreaDebug.DrawCubArea(this.position, this.size, this.color);
  }
}
