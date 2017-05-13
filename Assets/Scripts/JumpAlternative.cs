using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAlternative : MonoBehaviour
{

    [SerializeField]
    BezierCurve jumpCurve;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float jumpTime = 0.00001f;

    [SerializeField]
    bool requireInZone;

    public bool CanJump
    {
        get
        {
            return !requireInZone || inZone;
        }
    }

    bool inZone
    {
        get
        {
            if (zone != null && pt != null)
            {
                return zone.IsInside(pt.Curve, pt.CurveTime);
            }
            return false;
        }
    }
    BezierZone zone;
    BezierPoint pt;
    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        //Debug.Log(this + ": zone " + bEvent.zone + " is " + bEvent.type + " point " + bEvent.point.CurveTime);
        zone = bEvent.zone;
        pt = bEvent.point;
    }

    public void Grab(BezierPoint point)
    {
        point.Attach(jumpCurve, jumpTime);
    }
}