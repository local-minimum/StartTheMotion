using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {

    public Vector3[] points = new Vector3[3];   

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return
            2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);
    }

    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }

    public virtual int N
    {
        get
        {
            return points.Length;
        }
    }

    public virtual Vector3 GetComponentPoint(int index)
    {
        return points[index];
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            3f * oneMinusT * oneMinusT * (p1 - p0) +
            6f * oneMinusT * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }

    public virtual Vector3 GetPoint(float t)
    {
        switch (points.Length) {
            case 4:
                return GetPoint(points[0], points[1], points[2], points[3],  t);
            case 3:
                return GetPoint(points[0], points[1], points[2], t);
            case 2:
                return Vector3.Lerp(points[0], points[1], t);
            default:
                throw new System.ArgumentException("Must have 2-4 points");
        }
    }

    public Vector3 GetGlobalPoint(float t)
    {
        return transform.TransformPoint(GetPoint(t));
    }

    public float TimeAt(Vector3 point)
    {
        Vector3 local = transform.InverseTransformPoint(point);
        throw new System.NotImplementedException();
    }
}
