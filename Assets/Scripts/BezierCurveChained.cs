using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveChained : BezierCurve {

    public BezierCurve anchor;

    [Range(0, 1)]
    public float anchorTime;

    public Vector3 anchorPoint
    {
        get
        {
            return transform.InverseTransformPoint(anchor.GetGlobalPoint(anchorTime));
        }
    }

    public override int N
    {
        get
        {
            return base.N + 1;
        }
    }

    public override Vector3 GetComponentPoint(int index)
    {
        if (index == 0)
        {
            return anchorPoint;
        }
        else
        {
            return base.GetComponentPoint(index - 1);
        }
    }

    public override Vector3 GetPoint(float t)
    {
        switch (points.Length)
        {
            case 3:
                return GetPoint(anchorPoint, points[0], points[1], points[2], t);
            case 2:
                return GetPoint(anchorPoint, points[0], points[1], t);
            case 1:
                return Vector3.Lerp(anchorPoint, points[0], t);
            default:
                throw new System.ArgumentException("Must have 1-3 own points");
        }
    }

}
