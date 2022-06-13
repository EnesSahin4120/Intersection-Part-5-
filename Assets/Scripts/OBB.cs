using UnityEngine;
public class OBB : MonoBehaviour
{
    public Coordinates center; 

    public Coordinates localAxis_X;
    public Coordinates localAxis_Y;
    public Coordinates localAxis_Z;

    public float extent_X;
    public float extent_Y;
    public float extent_Z;

    public Coordinates[] localAxisIds = new Coordinates[3];
    public float[] extentIds = new float[3];
    public float[] minIds = new float[3];
    public float[] maxIds = new float[3];

    public OBB (Coordinates _center, Coordinates _localAxis_X, Coordinates _localAxis_Y, Coordinates _localAxis_Z, float _extent_X, float _extent_Y, float _extent_Z) 
    {
        center = _center;
        localAxis_X = _localAxis_X;
        localAxis_Y = _localAxis_Y;
        localAxis_Z = _localAxis_Z;
        extent_X = _extent_X;
        extent_Y = _extent_Y;
        extent_Z = _extent_Z;
        localAxisIds[0] = localAxis_X;
        localAxisIds[1] = localAxis_Y;
        localAxisIds[2] = localAxis_Z;
        extentIds[0] = extent_X;
        extentIds[1] = extent_Y;
        extentIds[2] = extent_Z;
    }
}
