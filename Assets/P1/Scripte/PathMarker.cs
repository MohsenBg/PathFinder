using UnityEngine;

public class PathMarker {
    public MapLocation Location;
    public GameObject Marker;
    public PathMarker Parent;

    // target Distance 
    public float G;

    // start Distance
    public float H;

    // total
    public float F;

    public PathMarker(MapLocation location, GameObject marker, PathMarker parent, float g, float h, float f) {
        this.Location = location;
        this.Marker = marker;
        this.Parent = parent;
        this.G = g;
        this.H = h;
        this.F = f;
    }

    public override bool Equals(object obj) {
        if (obj == null || !obj.GetType().Equals(GetType()))
            return false;

        MapLocation marketLocation = ((PathMarker)obj).Location;

        return marketLocation.Equals(Location);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public void Log() {
        Debug.Log("==============================================\n");
        Debug.Log($"location:(x:{this.Location.x} z:{this.Location.z})\n (g:{this.G}, h:{this.H}, f:{this.F}) \n");
        Debug.Log("==============================================\n");
    }

    public override string ToString() {
        return base.ToString();
    }
}
