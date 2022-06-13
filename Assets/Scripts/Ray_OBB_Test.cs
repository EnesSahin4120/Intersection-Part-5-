using UnityEngine;

public class Ray_OBB_Test : MonoBehaviour
{
    [SerializeField] private Transform obbShape;

    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Transform rayDirection;

    private bool isIntersect;

    float minX, minY, minZ, maxX, maxY, maxZ;

    private void Start()
    {
        minX = minY = minZ = Mathf.Infinity;
        maxX = maxY = maxZ = Mathf.NegativeInfinity;
    }

    private void Update()
    {
        Coordinates rayOriginCoord = new Coordinates(rayOrigin.position.x, rayOrigin.position.y, rayOrigin.position.z);
        Coordinates rayDirCoord = new Coordinates(rayDirection.position.x, rayDirection.position.y, rayDirection.position.z);
        Line _ray = new Line(rayOriginCoord, rayDirCoord, Line.LINETYPE.RAY);

        Coordinates center = new Coordinates(obbShape.position.x, obbShape.position.y, obbShape.position.z);
        Coordinates localAxisX = new Coordinates(obbShape.right.x, obbShape.right.y, obbShape.right.z);
        Coordinates localAxisY = new Coordinates(obbShape.up.x, obbShape.up.y, obbShape.up.z);
        Coordinates localAxisZ = new Coordinates(obbShape.forward.x, obbShape.forward.y, obbShape.forward.z);
        float extentX = obbShape.localScale.x / 2f;
        float extentY = obbShape.localScale.y / 2f;
        float extentZ = obbShape.localScale.z / 2f;

        OBB _obb = new OBB(center, localAxisX, localAxisY, localAxisZ, extentX, extentY, extentZ);
        CalculateMinMaxParameters();
        _obb.minIds[0] = minX;
        _obb.minIds[1] = minY;
        _obb.minIds[2] = minZ;
        _obb.maxIds[0] = maxX;
        _obb.maxIds[1] = maxY;
        _obb.maxIds[2] = maxZ;

        isIntersect = Mathematics.IsIntersectRay_OBB(_obb, _ray, obbShape.localToWorldMatrix);
        if (isIntersect)
            Debug.DrawLine(rayOriginCoord.ToVector(), rayDirCoord.ToVector(), Color.red);
        else
            Debug.DrawLine(rayOriginCoord.ToVector(), rayDirCoord.ToVector(), Color.green);
    }

    private void CalculateMinMaxParameters()
    {
        Vector3[] _points = Mathematics.LocalToWorldVertices(obbShape.gameObject);
        foreach (Vector3 current in _points)
        {
            if (current.x < minX) minX = current.x;
            if (current.x > maxX) maxX = current.x;
            if (current.y < minY) minY = current.y;
            if (current.y > maxY) maxY = current.y;
            if (current.z < minZ) minZ = current.z;
            if (current.z > maxZ) maxZ = current.z;
        }
    }
}
