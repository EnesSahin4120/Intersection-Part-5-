using UnityEngine;

public class Plane_OBB_Test : MonoBehaviour
{
    [SerializeField] private Transform obbShape;
    [SerializeField] private Transform planeShape;

    private bool isIntersect;

    private void Update()
    {
        Coordinates center = new Coordinates(obbShape.position.x, obbShape.position.y, obbShape.position.z);

        Coordinates localAxisX = new Coordinates(obbShape.right.x, obbShape.right.y, obbShape.right.z);
        Coordinates localAxisY = new Coordinates(obbShape.up.x, obbShape.up.y, obbShape.up.z);
        Coordinates localAxisZ = new Coordinates(obbShape.forward.x, obbShape.forward.y, obbShape.forward.z);
        float extentX = obbShape.localScale.x / 2f;
        float extentY = obbShape.localScale.y / 2f;
        float extentZ = obbShape.localScale.z / 2f;

        OBB _obb = new OBB(center, localAxisX, localAxisY, localAxisZ, extentX, extentY, extentZ);

        isIntersect = Mathematics.IsIntersectPlane_OBB(planeShape, _obb);
        if (isIntersect)
            SetColor(obbShape.gameObject, Color.red);
        else
            SetColor(obbShape.gameObject, Color.green);
    }

    private void SetColor(GameObject shape, Color color)
    {
        shape.GetComponent<MeshRenderer>().material.color = color;
    }
}
