﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveChained : BezierCurve {

    public BezierCurve anchor;

    [Range(0, 1)]
    public float anchorTime;

    public Vector3 AnchorPoint
    {
        get
        {
            return transform.InverseTransformPoint(anchor.GetGlobalPoint(anchorTime));
        }
    }

    public Vector3 GlobalAnchorPoint
    {
        get
        {
            return anchor.GetGlobalPoint(anchorTime);
        }
    }

    public override int N
    {
        get
        {
            return base.N + 1;
        }
    }

    public override Quaternion GetRotationAt(float t)
    {
        Vector3 tangent = transform.TransformDirection(GetFirstDerivative(AnchorPoint, points[0], points[1], points[2], t)).normalized;
        Vector3 forward = transform.TransformDirection(transform.forward);
        return Quaternion.LookRotation(forward, Quaternion.AngleAxis(90, forward) * tangent);
    }

    public override void MakeLinear()
    {
        switch (N)
        {
            case 3:
                points[0] = Vector3.Lerp(AnchorPoint, points[1], 0.5f);
                break;
            case 4:
                points[0] = Vector3.Lerp(AnchorPoint, points[2], 0.25f);
                points[1] = Vector3.Lerp(AnchorPoint, points[2], 0.75f);
                break;
        }
    }

    public override void SetDefaultShape()
    {
        Vector3 pt = AnchorPoint;
        points = new Vector3[3]
        {
            pt + new Vector3(0.5f, 0.5f),
            pt + new Vector3(0.5f, -0.5f),
            pt + new Vector3(1f, 0f)
        };
    }

    public override Vector3 GetComponentPoint(int index)
    {
        if (index == 0)
        {
            return AnchorPoint;
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
                return GetPoint(AnchorPoint, points[0], points[1], points[2], t);
            case 2:
                return GetPoint(AnchorPoint, points[0], points[1], t);
            case 1:
                return Vector3.Lerp(AnchorPoint, points[0], t);
            default:
                throw new System.ArgumentException("Must have 1-3 own points");
        }
    }

}