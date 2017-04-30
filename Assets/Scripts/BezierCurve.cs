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
   
    public Vector3 GetComponentPointGlobal(int index)
    {
        return transform.TransformPoint(GetComponentPoint(index));
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

    public virtual Quaternion GetRotationAt(float t)
    {
        Vector3 tangent = transform.TransformDirection(GetFirstDerivative(points[0], points[1], points[2], points[3], t)).normalized;
        Vector3 forward = transform.TransformDirection(transform.forward);
        return Quaternion.LookRotation(forward, Quaternion.AngleAxis(90, forward) * tangent);
    }

    private const float minDelta = 0.002f;

    public float TimeClosestTo(Vector3 point)
    {
        Vector3 local = transform.InverseTransformPoint(point);
        float seed = GetSeedTime(local);

        Vector3 clostestPt = GetPoint(seed);
        float closestSq = Vector3.SqrMagnitude(local - clostestPt);
        float delta = 0.5f / seedSteps;

        while (true)
        {
            float lTime = Mathf.Max(0, seed - delta);
            float rTime = Mathf.Min(1, seed + delta);
            float sqDestLeft = Vector3.SqrMagnitude(GetPoint(lTime) - local);
            float sqDistRight = Vector3.SqrMagnitude(GetPoint(rTime) - local);
            if (sqDestLeft < sqDistRight && sqDestLeft < closestSq)
            {
                closestSq = sqDestLeft;
                seed = lTime;
            } else if (sqDistRight < closestSq)
            {
                closestSq = sqDistRight;
                seed = rTime;
            }
            delta *= 0.5f;
            if (delta < minDelta)
            {
                break;
            }            
        }
        return seed;
    }

    public Vector3 GetClosestPointOnBezier(Vector3 worldPos)
    {
        return GetGlobalPoint(TimeClosestTo(worldPos));
    }

    private const int seedSteps = 42;

    float GetSeedTime(Vector3 localPt)
    {
        float seed = 0;
        float minSqDist = -1;
        for (int i =0; i<=seedSteps; i++)
        {
            float t = i / (float) seedSteps;
            //Debug.Log(t);
            float sqDist = Vector3.SqrMagnitude(localPt - GetPoint(t));
            if (minSqDist < 0 || sqDist < minSqDist)
            {
                seed = t;
                minSqDist = sqDist;
            }
        }
        return seed;
    }


    [SerializeField]
    float m_cachedLength;

    public float Length
    {
        get
        {
            return m_cachedLength;
        }
    }

    private const float maxLengthStep = 0.01f;
    private const float tStartStep = 0.01f;

    public void CalculateLength()
    {
        Vector3 prev = GetGlobalPoint(0);
        Vector3 end = GetGlobalPoint(1);
        Vector3 cur = prev;
        float tPrev = 0;
        float l = 0;
        float maxLSq = Mathf.Pow(Vector3.Distance(prev, end) * maxLengthStep, 2);
        while (true)
        {
            float tCur = tPrev + tStartStep;
            tCur = Mathf.Min(tCur, 1.0f);

            do
            {
                cur = GetGlobalPoint(tCur);
                if (Vector3.SqrMagnitude(prev - cur) > maxLSq)
                {
                    tCur = Mathf.Lerp(tPrev, tCur, 0.5f);
                }
                else
                {
                    break;
                }
            } while (true);

            l += Vector3.Distance(prev, cur);
            prev = cur;
            tPrev = tCur;
            if (tCur >= 1.0f)
            {
                break;
            }
        }
        m_cachedLength = l;
    }
}
