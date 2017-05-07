using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection {None, Left, Right};

public class BezierPoint : MonoBehaviour {

    MoveDirection moveDirection = MoveDirection.None;

    public MoveDirection GetDirection()
    {
        return moveDirection;
    }

    [SerializeField]
    BezierCurve curve;

    [HideInInspector]
    public Vector3 anchorOffset;

    List<BezierZone> inZones = new List<BezierZone>();
    List<BezierZone> currentInZones = new List<BezierZone>();

    [SerializeField]
    bool rotateWithCurve;
    
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
            Debug.Log("Detaching from" + curve);
            curve.SendMessage("OnDetachFromCurve", this, SendMessageOptions.DontRequireReceiver);

            curve = null;
            List<BezierZone> tmpZones = new List<BezierZone>();
            tmpZones.AddRange(inZones);
            inZones.Clear();
            for (int i=0, l=tmpZones.Count; i< l; i++)
            {
                SendEvent(tmpZones[i], BezierZoneEventType.ExitZone);
            }            
        }

    }

    public void Attach(BezierCurve curve, float time)
    {
        Detatch();
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

    [HideInInspector]
    public bool forceAttachment = true;

    
    private void OnEnable()
    {
        forceAttachment = true;
    }

    private void OnDisable()
    {
        inZones.Clear();
    }

    

    public Vector3 CurvePoint
    {
        get
        {
            if (curve)
            {
                return curve.GetGlobalPoint(curveTime);
            } else
            {
                return Vector3.zero;
            }
        }
    }

    [SerializeField, Range(0, 1)]
    float curveTime;

    public float CurveTime
    {
        get
        {
            return curveTime;
        }

        set
        {
            float prev = curveTime;
            curveTime = Mathf.Clamp01(value);

            if (prev < curveTime)
            {
                moveDirection = MoveDirection.Right;
            } else if (prev > curveTime)
            {
                moveDirection = MoveDirection.Left;
            } else
            {
                moveDirection = MoveDirection.None;
            }
            transform.position = curve.GetGlobalPoint(curveTime) + transform.TransformDirection(anchorOffset);

            if (rotateWithCurve)
            {
                Vector3 euler = curve.GetRotationAt(curveTime).eulerAngles;
                if (euler.z < 0)
                {
                    euler.z += 360f;
                }
                //transform.rotation = curve.GetRotationAt(curveTime);
                transform.rotation = Quaternion.Euler(euler);
            }

            if (Application.isPlaying)
            {
                CheckEvents();
            }
        }
    }

    void SendEvent(BezierZone zone, BezierZoneEventType eventType)
    {
        MonoBehaviour component = zone;
        var type = component.GetType();
        var methodInfo = type.GetMethod("OnBezierZoneEvent");
        if (methodInfo == null)
        {
            Debug.LogWarning(zone + " is missing OnBezierZoneEvent");
        }
        else
        {
            //Debug.Log(eventType + ": " + this + " -> " + zone);
            methodInfo.Invoke(component, new object[] {
                    new BezierZoneEvent(zone, this, eventType)
                });
        }

    }

    void CheckEvents()
    {

        BezierZone.PointIsInZones(this, currentInZones);
        List<BezierZone> previousInZones = new List<BezierZone>();
        previousInZones.AddRange(inZones);
        inZones.Clear();

        for(int i=0, l=previousInZones.Count; i<l;i++)
        {
            if (!currentInZones.Contains(previousInZones[i]))
            {
                SendEvent(previousInZones[i], BezierZoneEventType.ExitZone);
            }
        }

        for (int i=0, l=currentInZones.Count; i<l; i++)
        {
            if (previousInZones.Contains(currentInZones[i]))
            {
                SendEvent(currentInZones[i], BezierZoneEventType.StayZone);
            }
            else
            {
                SendEvent(currentInZones[i], BezierZoneEventType.EnterZone);
            }
        }

        if (curveTime == 0 || curveTime == 1)
        {
            if (curve)
            {
                curve.SendMessage("OnBezierEnd", this, SendMessageOptions.DontRequireReceiver);
            }
            SendMessage("OnBezierEnd", this, SendMessageOptions.DontRequireReceiver);
        }

        inZones.AddRange(currentInZones);
    }

    public void Snap()
    {
        if (curve != null)
        {
            CurveTime = curveTime;
        }
    }

    private void Update()
    {
        if (forceAttachment)
        {
            CurveTime = curveTime;
            forceAttachment = false;
        }
    }
}
