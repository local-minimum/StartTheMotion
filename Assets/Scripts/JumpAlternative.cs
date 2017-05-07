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
            Debug.Log(inZone);
            return !requireInZone || inZone;
        }
    }

    bool inZone = false;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        //Debug.Log(this + ": zone " + bEvent.zone + " is " + bEvent.type + " point " + bEvent.point.CurveTime);
        inZone = bEvent.type != BezierZoneEventType.ExitZone;
    }

    public void Grab(BezierPoint point)
    {
        point.Attach(jumpCurve, jumpTime);
    }
}