using UnityEngine;

public class Mathematics : MonoBehaviour
{
    static public float Square(float grade)
    {
        return grade * grade;
    }

    static public float Distance(Coordinates coord1, Coordinates coord2)
    {
        float diffSquared = Square(coord1.x - coord2.x) +
            Square(coord1.y - coord2.y) +
            Square(coord1.z - coord2.z);
        float squareRoot = Mathf.Sqrt(diffSquared);
        return squareRoot;
    }

    static public float VectorLength(Coordinates vector)
    {
        float length = Distance(new Coordinates(0, 0, 0), vector);
        return length;
    }

    static public Coordinates Normalize(Coordinates vector)
    {
        float length = VectorLength(vector);
        vector.x /= length;
        vector.y /= length;
        vector.z /= length;

        return vector;
    }

    static public float Dot(Coordinates vector1, Coordinates vector2)
    {
        return (vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
    }

    static public Coordinates Projection(Coordinates vector1, Coordinates vector2)
    {
        float numeratorPart = Dot(vector1, vector2);
        float vector2Length = VectorLength(vector2);
        float denominatorPart = Square(vector2Length);
        Coordinates resultCoordinate = new Coordinates(vector2.x * (numeratorPart / denominatorPart), vector2.y * (numeratorPart / denominatorPart), vector2.z * (numeratorPart / denominatorPart));

        return resultCoordinate;
    }

    static public Vector3[] LocalToWorldVertices(GameObject shape)
    {
        MeshFilter meshfilter = shape.GetComponent<MeshFilter>();
        Mesh sharedMesh = meshfilter.sharedMesh;
        Matrix4x4 localtoWorldMatrix = shape.transform.localToWorldMatrix;
        Vector3[] resultVertices = new Vector3[sharedMesh.vertices.Length];
        for (int i = 0; i < resultVertices.Length; i++)
        {
            resultVertices[i] = localtoWorldMatrix.MultiplyPoint3x4(sharedMesh.vertices[i]);
        }
        return resultVertices;
    }

    static public bool IsIntersectOBB_OBB(OBB obb1, OBB obb2)
    {
        float proj1, proj2; 
        float[,] matrixElements = new float[3, 3];
        float[,] abs_matrixElements = new float[3, 3];
    

        //Computing Rotation Matrix
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                matrixElements[i, j] = Dot(obb1.localAxisIds[i], obb2.localAxisIds[j]);

        //Translate Vector
        Coordinates t = obb2.center - obb1.center;
        t = new Coordinates(Dot(t, obb1.localAxis_X), Dot(t, obb1.localAxis_Y), Dot(t, obb1.localAxis_Z));
        float[] t_Ids = new float[3];
        t_Ids[0] = t.x;
        t_Ids[1] = t.y;
        t_Ids[2] = t.z;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                abs_matrixElements[i, j] = Mathf.Abs(matrixElements[i, j]);

        //Axes Testing obb1
        for(int i = 0; i < 3; i++)
        {
            proj1 = obb1.extentIds[i];
            proj2 = obb2.extent_X * abs_matrixElements[i, 0] + obb2.extent_Y * abs_matrixElements[i, 1] + obb2.extent_Z * abs_matrixElements[i, 2];
            if (Mathf.Abs(t_Ids[i]) > proj1 + proj2)
                return false;
        }
         
        //Axes Testing obb2
        for(int i = 0; i < 3; i++)
        {
            proj1 = obb1.extent_X * abs_matrixElements[0, i] + obb1.extent_Y * abs_matrixElements[1, i] + obb1.extent_Z * abs_matrixElements[2, i];
            proj2 = obb2.extentIds[i];
            if (Mathf.Abs(t.x * matrixElements[0, i] + t.y * matrixElements[1, i] + t.z * matrixElements[2, i]) > proj1 + proj2)
                return false;
        }

        //Axis Testing obb1X x obb2X
        proj1 = obb1.extent_Y * abs_matrixElements[2, 0] + obb1.extent_Z * abs_matrixElements[1, 0];
        proj2 = obb2.extent_Y * abs_matrixElements[0, 2] + obb2.extent_Z * abs_matrixElements[0, 1];
        if (Mathf.Abs(t.z * matrixElements[1, 0] - t.y * matrixElements[2, 0]) > proj1 + proj2)
            return false;

        //Axis Testing obb1X x obb2Y
        proj1 = obb1.extent_Y * abs_matrixElements[2, 1] + obb1.extent_Z * abs_matrixElements[1, 1];
        proj2 = obb2.extent_X * abs_matrixElements[0, 2] + obb2.extent_Z * abs_matrixElements[0, 0];
        if (Mathf.Abs(t.z * matrixElements[1, 1] - t.y * matrixElements[2, 1]) > proj1 + proj2)
            return false;

        //Axis Testing obb1X x obb2Z
        proj1 = obb1.extent_Y * abs_matrixElements[2, 2] + obb1.extent_Z * abs_matrixElements[1, 2];
        proj2 = obb2.extent_X * abs_matrixElements[0, 1] + obb2.extent_Y * abs_matrixElements[0, 0];
        if (Mathf.Abs(t.z * matrixElements[1, 2] - t.y * matrixElements[2, 2]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Y x obb2X
        proj1 = obb1.extent_X * abs_matrixElements[2, 0] + obb1.extent_Z * abs_matrixElements[0, 0];
        proj2 = obb2.extent_Y * abs_matrixElements[1, 2] + obb2.extent_Z * abs_matrixElements[1, 1];
        if (Mathf.Abs(t.x * matrixElements[2, 1] - t.z * matrixElements[0, 1]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Y x obb2Y
        proj1 = obb1.extent_X * abs_matrixElements[2, 1] + obb1.extent_Z * abs_matrixElements[0, 1];
        proj2 = obb2.extent_X * abs_matrixElements[1, 2] + obb2.extent_Z * abs_matrixElements[1, 0];
        if (Mathf.Abs(t.x * matrixElements[2, 1] - t.z * matrixElements[0, 1]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Y x obb2Z
        proj1 = obb1.extent_X * abs_matrixElements[2, 2] + obb1.extent_Z * abs_matrixElements[0, 2];
        proj2 = obb2.extent_X * abs_matrixElements[1, 1] + obb2.extent_Y * abs_matrixElements[1, 0];
        if (Mathf.Abs(t.x * matrixElements[2, 2] - t.z * matrixElements[0, 2]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Z x obb2X
        proj1 = obb1.extent_X * abs_matrixElements[1, 0] + obb1.extent_Y * abs_matrixElements[0, 0];
        proj2 = obb2.extent_Y * abs_matrixElements[2, 2] + obb2.extent_Z * abs_matrixElements[2, 1];
        if (Mathf.Abs(t.y * matrixElements[0, 0] - t.x * matrixElements[1, 0]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Z x obb2Y
        proj1 = obb1.extent_X * abs_matrixElements[1, 1] + obb1.extent_Y * abs_matrixElements[0, 1];
        proj2 = obb2.extent_X * abs_matrixElements[2, 2] + obb2.extent_Z * abs_matrixElements[2, 0];
        if (Mathf.Abs(t.y * matrixElements[0, 1] - t.x * matrixElements[1, 1]) > proj1 + proj2)
            return false;

        //Axis Testing obb1Z x obb2Z
        proj1 = obb1.extent_X * abs_matrixElements[1, 2] + obb1.extent_Y * abs_matrixElements[0, 2];
        proj2 = obb2.extent_X * abs_matrixElements[2, 1] + obb2.extent_Y * abs_matrixElements[2, 0];
        if (Mathf.Abs(t.y * matrixElements[0, 2] - t.x * matrixElements[1, 2]) > proj1 + proj2)
            return false;

        return true;
    }

    static public bool IsIntersectRay_OBB(OBB obb, Line ray, Matrix4x4 transformationMatrix)
    {
        float tMin = Mathf.NegativeInfinity;
        float tMax = Mathf.Infinity;

        Coordinates delta = obb.center - ray.A;

        for (int i = 0; i < 3; i++)
        {
            Coordinates axis = new Coordinates(transformationMatrix.GetColumn(i).x, transformationMatrix.GetColumn(i).y, transformationMatrix.GetColumn(i).z);
            float e = Dot(axis, delta);
            float f = Dot(ray.v, axis);

            float t1 = (e - obb.extentIds[i]) / f;
            float t2 = (e + obb.extentIds[i]) / f;

            if (t1 > t2)
            {
                float w = t1;
                t1 = t2;
                t2 = w;
            }

            if (t2 < tMax)
                tMax = t2;

            if (t1 > tMin)
                tMin = t1;

            if (tMax < tMin)
                return false;
        }
        return true;
    }

    static public bool IsIntersectPlane_OBB(Transform plane, OBB obb)
    {
        Coordinates planeNormal = new Coordinates(plane.up.x, plane.up.y, plane.up.z);

        float r = obb.extent_X * Mathf.Abs(Dot(planeNormal, obb.localAxis_X)) +
                  obb.extent_Y * Mathf.Abs(Dot(planeNormal, obb.localAxis_Y)) +
                  obb.extent_Z * Mathf.Abs(Dot(planeNormal, obb.localAxis_Z));

        float s = Dot(planeNormal, obb.center) - plane.position.sqrMagnitude;

        return Mathf.Abs(s) <= r;
    }
}
