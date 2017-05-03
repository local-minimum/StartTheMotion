using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierZone : MonoBehaviour {

    static List<BezierZone> zones = new List<BezierZone>();

    private void Reset()
    {
        curve = GetComponent<BezierCurve>();
        times = new float[] { 0.25f, 0.75f };
#if UNITY_EDITOR

        lineColor = Color.red;

#endif

}

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
            if (zones[i].IsInside(pt.Curve, pt.CurveTime))
            {
                myZones.Add(zones[i]);
            }
        }
    }

    public MonoBehaviour[] targets;

    public T GetTarget<T>() where T : MonoBehaviour
    {
        var t = typeof(T);
        for (int i=0; i<targets.Length; i++)
        {
            var t2 = targets[i].GetType();
            if (t == t2 || t2.IsSubclassOf(t))
            {
                return targets[i] as T;
            }
        }
        return null;
    }

    public List<T> GetAllTargets<T>() where T : MonoBehaviour
    {
        var ret = new List<T>();
        var t = typeof(T);
        for (int i = 0; i < targets.Length; i++)
        {
            var t2 = targets[i].GetType();
            if (t == t2 || t2.IsSubclassOf(t))
            {
                ret.Add(targets[i] as T);
            }
        }
        return ret;
    }

    //[HideInInspector]
    public float[] times = new float[2] { 0.25f, .75f };

    public float left
    {
        get {
            return Mathf.Min(times[0], times[1]);
        }

    }

    public float right
    {
        get
        {
            return Mathf.Max(times[0], times[1]);
        }
    }

    public BezierCurve curve;

    public virtual bool IsInside(BezierCurve curve, float t)
    {

        return curve == this.curve && t >= left && t <= right;
    }

#if UNITY_EDITOR

    public Color lineColor;

#endif
}
