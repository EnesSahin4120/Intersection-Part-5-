using UnityEngine;

public class Line : MonoBehaviour
{
    public Coordinates A;
    public Coordinates B;
    public Coordinates v;

    public enum LINETYPE { LINE, SEGMENT, RAY };
    LINETYPE type;

    public Line(Coordinates _A, Coordinates _B, LINETYPE _type)
    {
        A = _A;
        B = _B;
        type = _type;
        v = new Coordinates(B.x - A.x, B.y - A.y, B.z - A.z);
    }

    public Line(Coordinates _A, Coordinates _V)
    {
        A = _A;
        v = _V;
        B = _A + v;
        type = LINETYPE.SEGMENT;
    }

    public Coordinates Lerp(float t)
    {
        if (type == LINETYPE.SEGMENT)
            t = Mathf.Clamp(t, 0, 1);
        else if (type == LINETYPE.RAY && t < 0)
            t = 0;

        float xt = A.x + v.x * t;
        float yt = A.y + v.y * t;
        float zt = A.z + v.z * t;

        return new Coordinates(xt, yt, zt);
    }
}