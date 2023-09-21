using System.Collections.Generic;
using UnityEngine;
using MyBox;


[ExecuteAlways]
public class Zone : MonoBehaviour
{
  [SerializeField][ReadOnly] private const string DEFUALT_LAYER = "Ignore Raycast";
  [SerializeField][ReadOnly] private string _name = "Zone";
  [SerializeField][ReadOnly] private Vector3 _position = Vector3.zero;
  [SerializeField][ReadOnly] private Vector3 _size = new Vector3(1, 1, 1);
  [SerializeField][ReadOnly] private float _cost = 1f;
  [SerializeField][ReadOnly] private string _areaLayer = DEFUALT_LAYER;

  public string Name
  {
    get { return _name; }
    private set { _name = value; }
  }

  public Vector3 Position
  {
    get { return _position; }
    private set { _position = value; }
  }


  public Vector3 Size
  {
    get { return _size; }
    private set { _size = value; }
  }


  public float Cost
  {
    get { return _cost; }
    private set { _cost = value; }
  }


  public string AreaLayer
  {
    get { return _areaLayer; }
    private set { _areaLayer = value; }
  }


  public void SetZoneData(ZoneData zoneData)
  {
    Name = zoneData.name;
    Position = zoneData.position;
    Size = zoneData.size;
    Cost = zoneData.cost;
    LayerInfo layer = new LayersFilter(zoneData.areaLayer).FirstLayer;
    AreaLayer = layer.Index != -1 ? layer.Name : DEFUALT_LAYER;
    UpdateZone();
  }


  private void Update()
  {
    UpdateZone();
  }


  private void UpdateZone()
  {
    transform.position = this.Position;
    transform.localScale = this.Size;
    gameObject.name = this.Name;
    gameObject.layer = LayerMask.NameToLayer(this.AreaLayer);
  }
}
