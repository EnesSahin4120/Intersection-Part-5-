using UnityEngine;

public class OBB_OBB_Test : MonoBehaviour
{
    [SerializeField] private Transform obbShape1;
    [SerializeField] private Transform obbShape2;
    private bool isIntersect;

    private void Update() 
    {
        Coordinates center_1 = new Coordinates(obbShape1.position.x, obbShape1.position.y, obbShape1.position.z); 
        Coordinates localAxisX_1 = new Coordinates(obbShape1.right.x, obbShape1.right.y, obbShape1.right.z);
        Coordinates localAxisY_1 = new Coordinates(obbShape1.up.x, obbShape1.up.y, obbShape1.up.z);
        Coordinates localAxisZ_1 = new Coordinates(obbShape1.forward.x, obbShape1.forward.y, obbShape1.forward.z);
        float extentX_1 = obbShape1.localScale.x / 2f;
        float extentY_1 = obbShape1.localScale.y / 2f;
        float extentZ_1 = obbShape1.localScale.z / 2f;

        Coordinates center_2 = new Coordinates(obbShape2.position.x, obbShape2.position.y, obbShape2.position.z);
        Coordinates localAxisX_2 = new Coordinates(obbShape2.right.x, obbShape2.right.y, obbShape2.right.z);
        Coordinates localAxisY_2 = new Coordinates(obbShape2.up.x, obbShape2.up.y, obbShape2.up.z);
        Coordinates localAxisZ_2 = new Coordinates(obbShape2.forward.x, obbShape2.forward.y, obbShape2.forward.z);
        float extentX_2 = obbShape2.localScale.x / 2f;
        float extentY_2 = obbShape2.localScale.y / 2f;
        float extentZ_2 = obbShape2.localScale.z / 2f;

        OBB obb1 = new OBB(center_1, localAxisX_1, localAxisY_1, localAxisZ_1, extentX_1, extentY_1, extentZ_1);
        OBB obb2 = new OBB(center_2, localAxisX_2, localAxisY_2, localAxisZ_2, extentX_2, extentY_2, extentZ_2);

        isIntersect = Mathematics.IsIntersectOBB_OBB(obb1, obb2);
        if (isIntersect)
            SetColor(obbShape1.gameObject, Color.red);
        else
            SetColor(obbShape1.gameObject, Color.green);
    }

    private void SetColor(GameObject shape, Color color)
    {
        shape.GetComponent<MeshRenderer>().material.color = color;
    }
}
