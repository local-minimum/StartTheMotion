using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour {

    [SerializeField]
    BezierCurve curve;

    [HideInInspector]
    public Vector3 anchorOffset;

    List<BezierZone> inZones = new List<BezierZone>();
    List<BezierZone> tmpZones = new List<BezierZone>();

    [SerializeField]
    bool rotateWithCurve;

    public void SwapAnchor(BezierCurve curve, float time)
    {
        this.curve = curve;
        CurveTime = time;
    }
    
    public BezierCurve Curve
    {
        get
        {
            return curve;
        }        
    }

    public void Detatch()
    {
        if (curve)
        {
            curve.SendMessage("OnDetachFromCurve", this, SendMessageOptions.DontRequireReceiver);
        }
        inZones.Clear();
        curve = null;
    }

    public void ReAttach(BezierCurve curve, float time)
    {
        if (this.curve)
        {
            this.curve.SendMessage("OnDetachFromCurve", this, SendMessageOptions.DontRequireReceiver);
        }
        inZones.Clear();
        this.curve = curve;
        curve.SendMessage("OnAttachToCurve", this, SendMessageOptions.DontRequireReceiver);
        CurveTime = time;
    }

    public void Move(float distance)
    {
        bool hitEnd;
        if (curve)
        {
            CurveTime = curve.GetTimeAfter(curveTime, distance, out hitEnd);
        }
    }

    private void OnEnable()
    {
        CurveTime = curveTime;
    }

    private void OnDisable()
    {
        inZones.Clear();
    }

    

    public Vector3 CurvePoint
    {
        get
        {
            return curve.GetGlobalPoint(curveTime);
        }
    }

    [SerializeField]
    float curveTime;

    public float CurveTime
    {
        get
        {
            return curveTime;
        }

        set
        {
            curveTime = Mathf.Clamp01(value);

            transform.position = curve.GetGlobalPoint(curveTime) + transform.TransformDirection(anchorOffset);

            if (rotateWithCurve)
            {
                transform.rotation = curve.GetRotationAt(curveTime);
            }

            if (Application.isPlaying)
            {
                CheckEvents();
            }
        }
    }

    void CheckEvents()
    {

        BezierZone.PointIsInZones(this, tmpZones);
        for(int i=0, l=inZones.Count; i<l;i++)
        {
            if (!tmpZones.Contains(inZones[i]))
            {
                MonoBehaviour component = inZones[i];
                var type = component.GetType();
                var methodInfo = type.GetMethod("OnBezierZoneEvent");
                if (methodInfo == null)
                {
                    Debug.LogWarning(inZones[i] + " is missing OnBezierZoneEvent");
                }
                else
                {
                    methodInfo.Invoke(component, new object[] {
                    new BezierZoneEvent(inZones[i], this, BezierZoneEventType.ExitZone)
                });
                }
                /*
                inZones[i].SendMessage(
                    "OnBezierZoneEvent", 
                    new BezierZoneEvent(inZones[i], this, BezierZoneEventType.ExitZone),
                    SendMessageOptions.DontRequireReceiver);
                    */
            }
        }

        for (int i=0, l=tmpZones.Count; i<l; i++)
        {
            if (inZones.Contains(tmpZones[i]))
            {
                tmpZones[i].SendMessage(
                    "OnBezierZoneEvent",
                    new BezierZoneEvent(tmpZones[i], this, BezierZoneEventType.StayZone),
                    SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tmpZones[i].SendMessage(
                    "OnBezierZoneEvent",
                    new BezierZoneEvent(tmpZones[i], this, BezierZoneEventType.EnterZone),
                    SendMessageOptions.DontRequireReceiver);
            }
        }
        inZones.Clear();
        inZones.AddRange(tmpZones);

        if (curveTime == 0 || curveTime == 1)
        {
            curve.SendMessage("OnBezierEnd", this, SendMessageOptions.DontRequireReceiver);
        }


    }

    public void Snap()
    {
        if (curve != null)
        {
            CurveTime = curveTime;
        }
    }
}
