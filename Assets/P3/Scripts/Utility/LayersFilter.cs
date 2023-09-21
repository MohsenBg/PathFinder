
using System.Collections.Generic;
using UnityEngine;

public struct LayerInfo
{
  public string Name { get; }
  public int Index { get; }

  public LayerInfo(string name, int index)
  {
    Name = name;
    Index = index;
  }
}

public class LayersFilter
{
  public List<LayerInfo> Layers { get; }

  public LayerInfo FirstLayer => Layers.Count > 0 ? Layers[0] : new LayerInfo("No Layers", -1);
  public LayerInfo LastLayer => Layers.Count > 0 ? Layers[Layers.Count - 1] : new LayerInfo("No Layers", -1);

  public LayersFilter(LayerMask layerMask)
  {
    Layers = new List<LayerInfo>();
    for (int i = 0; i < 32; i++)
    {
      int layerValue = 1 << i;

      if ((layerMask & layerValue) != 0)
      {
        Layers.Add(new LayerInfo(LayerMask.LayerToName(i), i));
      }
    }
  }
}

