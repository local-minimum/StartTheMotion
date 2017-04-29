using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierZone : MonoBehaviour {

    static List<BezierZone> zones = new List<BezierZone>();

    private void OnEnable()
    {
        zones.Add(this);
    }

    private void OnDisable()
    {
        zones.Remove(this);
    }

    private void OnDestroy()
    {
        zones.Remove(this);
    }

    public static void PointIsInZones(BezierPoint pt, List<BezierZone> myZones)
    {
        myZones.Clear();   
        for (int i=0, l=zones.Count; i<l; i++)
        {
            if (pt.IsOnCurve(zones[i].curve) && zones[i].IsInside(pt.CurveTime))
            {
                myZones.Add(zones[i]);
            }
        }
    }

    public float[] times = new float[2] { 0, 1 };
    public float left
    {
        get {
            return times[0];
        }

    }

    public float right
    {
        get
        {
            return times[1];
        }
    }

    public BezierCurve curve;

    public bool IsInside(float t)
    {
        return t >= left && t <= right;
    }

#if UNITY_EDITOR

    public Color lineColor;

#endif
}
