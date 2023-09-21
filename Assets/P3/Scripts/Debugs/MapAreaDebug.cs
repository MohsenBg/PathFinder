using UnityEngine;

static public class MapAreaDebug
{

  static public void DrawCubArea(Vector3 position, Vector3 size, Color color)
  {
    float topY = position.y + size.y / 2;
    float bottomY = position.y - size.y / 2;

    float rightX = position.x + size.x / 2;
    float leftX = position.x - size.x / 2;

    float frontZ = position.z + size.z / 2;
    float backZ = position.z - size.z / 2;

    // Front square
    Vector3 rightBottomFront = new Vector3(rightX, bottomY, frontZ);
    Vector3 leftBottomFront = new Vector3(leftX, bottomY, frontZ);
    Vector3 rightTopFront = new Vector3(rightX, topY, frontZ);
    Vector3 leftTopFront = new Vector3(leftX, topY, frontZ);

    // Back square (corresponding points)
    Vector3 rightBottomBack = new Vector3(rightX, bottomY, backZ);
    Vector3 leftBottomBack = new Vector3(leftX, bottomY, backZ);
    Vector3 rightTopBack = new Vector3(rightX, topY, backZ);
    Vector3 leftTopBack = new Vector3(leftX, topY, backZ);

    // Draw lines for the front square
    Debug.DrawLine(rightBottomFront, leftBottomFront, color);
    Debug.DrawLine(rightTopFront, leftTopFront, color);
    Debug.DrawLine(rightBottomFront, rightTopFront, color);
    Debug.DrawLine(leftBottomFront, leftTopFront, color);

    // Draw lines connecting front and back squares
    Debug.DrawLine(rightBottomFront, rightBottomBack, color);
    Debug.DrawLine(leftBottomFront, leftBottomBack, color);
    Debug.DrawLine(rightTopFront, rightTopBack, color);
    Debug.DrawLine(leftTopFront, leftTopBack, color);

    // Draw lines for the back square
    Debug.DrawLine(rightBottomBack, leftBottomBack, color);
    Debug.DrawLine(rightTopBack, leftTopBack, color);
    Debug.DrawLine(rightBottomBack, rightTopBack, color);
    Debug.DrawLine(leftBottomBack, leftTopBack, color);
  }

}
