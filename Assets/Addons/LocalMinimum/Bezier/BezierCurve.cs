using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BezierType {Quadratic, Cubic};

public class BezierCurve : MonoBehaviour {
    
    public BezierType bezierType = BezierType.Cubic;

    [HideInInspector]
    public Vector3[] points = new Vector3[4];

    [SerializeField]
    bool attractorsAreOffsets;

    #region LeftAnchor
    public BezierCurve anchorLeft;

    [Range(0, 1)]
    public float anchoredTimeLeft;

    Vector3 AnchorPointLeft
    {
        get
        {
            return transform.InverseTransformPoint(anchorLeft.GetGlobalPoint(anchoredTimeLeft));
        }
    }

    public Vector3 GlobalAnchorPointLeft
    {
        get
        {
            return anchorLeft.GetGlobalPoint(anchoredTimeLeft);
        }
    }

    #endregion

    #region RightAnchor
    public BezierCurve anchorRight;

    [Range(0, 1)]
    public float anchoredTimeRight;

    Vector3 AnchorPointRight
    {
        get
        {
            return transform.InverseTransformPoint(anchorRight.GetGlobalPoint(anchoredTimeRight));
        }
    }

    public Vector3 GlobalAnchorPointRight
    {
        get
        {
            return anchorRight.GetGlobalPoint(anchoredTimeRight);
        }
    }

    #endregion

    #region StaticBezierCalculations

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

    #endregion

    #region Points
    public virtual Vector3 GetPoint(float t)
    {
        switch (bezierType)
        {
            case BezierType.Cubic:
                return GetPoint(GetComponentPoint(0), GetComponentPoint(1), GetComponentPoint(2), GetComponentPoint(3), t);
            case BezierType.Quadratic:
                return GetPoint(GetComponentPoint(0), GetComponentPoint(1), GetComponentPoint(3), t);
            default:
                throw new System.ArgumentException("Wacky bezier");
        }
    }

    public Vector3 GetGlobalPoint(float t)
    {
        return transform.TransformPoint(GetPoint(t));
    }

    public void SetComponentPoint(int index, Vector3 globalPoint)
    {
        if (attractorsAreOffsets && (index == 1 || index == 2))
        {
            if (index == 1) {
                if (bezierType == BezierType.Quadratic) {
                    Debug.LogWarning("This isn'treally supported, just using offset on left side for now");
                }
                points[1] = transform.InverseTransformPoint(globalPoint) - GetComponentPoint(0);

            } else
            {
                points[2] = transform.InverseTransformPoint(globalPoint) - GetComponentPoint(3);
            }
        } else
        {
            points[index] = transform.InverseTransformPoint(globalPoint);
        }
    }

    public virtual Vector3 GetComponentPoint(int index)
    {
        switch (index)
        {
            case 0:
                return anchorLeft == null ? points[0] : AnchorPointLeft;
            case 1:
                if (attractorsAreOffsets)
                {
                    if (bezierType == BezierType.Cubic)
                    {
                        return GetComponentPoint(0) + points[1];
                    } else
                    {
                        Debug.LogWarning("Not really implemented, using left side offset for now");
                        return GetComponentPoint(0) + points[1];
                    }
                }
                else {
                    return points[1];
                }
            case 2:
                if (attractorsAreOffsets)
                {
                    return GetComponentPoint(3) + points[2];
                }
                else {
                    return points[2];
                }
            case 3:
                return anchorRight == null ? points[3] : AnchorPointRight;
        }
        throw new System.ArgumentException(index + " not in range 0-3");
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

    public Vector3 GetClosestPointOnBezier(Vector3 worldPos)
    {
        return GetGlobalPoint(TimeClosestTo(worldPos));
    }

    public void SetAllZTo(float value)
    {
        for (int i=0; i<4; i++)
        {
            points[i].z = value;
        }
    }

    public bool AllZIsZero
    {
        get
        {
            for (int i = 0; i < 4; i++)
            {
                if (points[i].z != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
    #endregion

    #region HelpFunctions
    public virtual void MakeLinear()
    {
        switch (bezierType)
        {
            case BezierType.Quadratic:
                points[1] = Vector3.Lerp(GetComponentPoint(0), GetComponentPoint(3), 0.5f);
                break;
            case BezierType.Cubic:
                points[1] = Vector3.Lerp(GetComponentPoint(0), GetComponentPoint(3), 0.25f);
                points[2] = Vector3.Lerp(GetComponentPoint(0), GetComponentPoint(3), 0.75f);
                break;
        }
    }

    public void SetDefaultShape()
    {
        Vector3 left = anchorLeft == null ? Vector3.zero : transform.InverseTransformPoint(AnchorPointLeft);
        Vector3 right = anchorRight == null ? left + Vector3.right : transform.InverseTransformPoint(AnchorPointRight);
        Vector3 midPoint = Vector3.Lerp(left, right, 0.5f);

        points = new Vector3[4]
        {
            left,
            midPoint + Vector3.up,
            midPoint + Vector3.down,
            right
        };
    }

    public void CloneMakeChild()
    {
        var GO = new GameObject(name + "-child");
        GO.transform.SetParent(transform);
        GO.transform.localPosition = Vector3.zero;
        var childCurve = GO.AddComponent<BezierCurve>();
        RelativeCloneToChild(this, childCurve);
    }

    static void RelativeCloneToChild(BezierCurve from, BezierCurve to)
    {
        to.anchoredTimeLeft = 1;
        to.anchorLeft = from;
        to.points = new Vector3[4];
        Vector3 toAnchor = to.AnchorPointLeft;
        Vector3 fromAnchor = from.GetGlobalPoint(0);
        for (int i = 0; i < from.points.Length; i++)
        {
            to.points[i] = to.transform.InverseTransformVector( 
                from.transform.TransformVector(from.points[i] - fromAnchor)) + toAnchor;
        }
    }

    public void InvertDirection()
    {
        Vector3 tmp = points[0];
        points[0] = points[3];
        points[3] = tmp;
        tmp = points[1];
        points[1] = points[2];
        points[2] = tmp;

        BezierCurve tmpA = anchorLeft;
        anchorLeft = anchorRight;
        anchorRight = tmpA;

        float tmpT = anchoredTimeLeft;
        anchoredTimeLeft = anchoredTimeRight;
        anchoredTimeRight = tmpT;
    }

    #endregion

    #region Times
    private const float maxLengthStep = 0.01f;
    private const float tStartStep = 0.01f;
    private const float lengthAcceptance = 0.001f;

    public float GetTimeAfter(float startTime, float distance, out bool hitEnd)
    {
        if (distance == 0)
        {
            hitEnd = false;
            return startTime;
        }

        int iterations = 0;
        Vector3 prev = GetGlobalPoint(startTime);        
        Vector3 cur = prev;
        float tPrev = startTime;
        float tCur = startTime;
        float direction = distance > 0 ? 1f : -1f;
        distance = Mathf.Abs(distance);
        float l = 0;
        float maxLSq = Mathf.Pow(Vector3.Distance(GetGlobalPoint(0), GetGlobalPoint(1)) * maxLengthStep, 2);
        while (true)
        {
            tCur = Mathf.Clamp01(tPrev + tStartStep * direction);            

            do
            {
                cur = GetGlobalPoint(tCur);
                if (Vector3.SqrMagnitude(prev - cur) > maxLSq)
                {
                    tCur = Mathf.Lerp(tPrev, tCur, 0.5f);
                }
                else
                {
                    float deltaL = Vector3.Distance(prev, cur);
                    if (l + deltaL > distance + lengthAcceptance && tCur != 0 && tCur != 1)
                    {
                        tCur = Mathf.Lerp(tPrev, tCur, 0.5f);
                    }
                    else
                    {
                        l += deltaL;
                        break;
                    }
                }

                iterations++;
                if (iterations > 1000)
                {
                    throw new System.ArgumentException(string.Format("Too many iterations finding {0}, now at {1} with l {2}", distance, tCur, l));
                }
            } while (true);

            prev = cur;
            tPrev = tCur;
            if (l > distance - lengthAcceptance || tCur == 0 || tCur == 1)
            {
                hitEnd = tCur == 0 || tCur == 1;
                break;
            }


        }

        return tCur;
    }

    private const float minDelta = 0.002f;

    public float TimeClosestTo(Vector3 point)
    {
        Vector3 local = transform.InverseTransformPoint(point);
        float seed = GetRoughClosestTime(local);

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
            }
            else if (sqDistRight < closestSq)
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

    private const int seedSteps = 42;

    float GetRoughClosestTime(Vector3 localPt)
    {
        float seed = 0;
        float minSqDist = -1;
        for (int i = 0; i <= seedSteps; i++)
        {
            float t = i / (float)seedSteps;
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

    #endregion

    #region Rotation
    public virtual Quaternion GetRotationAt(float t)
    {
        Vector3 tangent;
        switch (bezierType) {
            case BezierType.Cubic:
                tangent = transform.TransformDirection(
            GetFirstDerivative(GetComponentPoint(0), GetComponentPoint(1),
            GetComponentPoint(2), GetComponentPoint(3), t)).normalized;
                break;
            default:
                throw new System.NotImplementedException("No support for rotation of " + bezierType);
        }

        Vector3 forward = transform.TransformDirection(transform.forward);
        return Quaternion.LookRotation(forward, Quaternion.AngleAxis(90, forward) * tangent);
    }
    #endregion

    #region CurveLength
    [SerializeField]
    float m_cachedLength;

    public float Length
    {
        get
        {
            return m_cachedLength;
        }
    }

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
    #endregion
}
