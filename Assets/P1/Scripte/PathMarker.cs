using UnityEngine;

public class PathMarker {
    public MapLocation location;
    public GameObject marker;
    public PathMarker parent;

    // target Distance
    public float g;
    // start Distance
    public float h;
    // total
    public float f;

    public PathMarker(MapLocation location, GameObject marker, PathMarker parent, float g, float h, float f) {
        this.location = location;
        this.marker = marker;
        this.parent = parent;
        this.g = g;
        this.h = h;
        this.f = f;
    }

    public override bool Equals(object obj) {
        if (obj == null || !obj.GetType().Equals(GetType()))
            return false;

        MapLocation marketLocation = ((PathMarker)obj).location;

        return marketLocation.Equals(location);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public void Log() {
        Debug.Log("==============================================\n");
        Debug.Log($"location:(x:{this.location.x} z:{this.location.z})\n (g:{this.g}, h:{this.h}, f:{this.f}) \n");
        Debug.Log("==============================================\n");
    }

    public override string ToString() {
        return base.ToString();
    }
}
