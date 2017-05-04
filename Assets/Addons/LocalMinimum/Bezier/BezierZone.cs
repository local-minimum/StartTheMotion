using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BezierZoneEventType {EnterZone, StayZone, ExitZone};

[System.Serializable]
public struct BezierZoneEvent
{
    public BezierZone zone;
    public BezierPoint point;
    public BezierZoneEventType type;

    public BezierZoneEvent(BezierZone zone, BezierPoint point, BezierZoneEventType type)
    {
        this.zone = zone;
        this.point = point;
        this.type = type;
    }
}

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

    public MonoBehaviour[] forwardEventsTo;

    public T GetTarget<T>() where T : MonoBehaviour
    {
        var t = typeof(T);
        for (int i=0; i<forwardEventsTo.Length; i++)
        {
            var t2 = forwardEventsTo[i].GetType();
            if (t == t2 || t2.IsSubclassOf(t))
            {
                return forwardEventsTo[i] as T;
            }
        }
        return null;
    }

    public List<T> GetAllTargets<T>() where T : MonoBehaviour
    {
        var ret = new List<T>();
        var t = typeof(T);
        for (int i = 0; i < forwardEventsTo.Length; i++)
        {
            var t2 = forwardEventsTo[i].GetType();
            if (t == t2 || t2.IsSubclassOf(t))
            {
                ret.Add(forwardEventsTo[i] as T);
            }
        }
        return ret;
    }

    void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (bEvent.zone == this)
        {
            
            for (int i = 0, l = forwardEventsTo.Length; i < l; i++)
            {
                if (forwardEventsTo[i].gameObject == gameObject)
                {
                    Debug.LogWarning(
                        string.Format(
                            "Zone {0} and behaviour {1} is on same game object, no need to forward",
                            this, forwardEventsTo[i]));
                } else
                {
                    forwardEventsTo[i].SendMessage("OnBezierZoneEvent", bEvent);
                }
            }
        }
    }

    [HideInInspector]
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
