using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour {

    [SerializeField]
    BezierCurve curve;

    List<BezierZone> inZones = new List<BezierZone>();
    List<BezierZone> tmpZones = new List<BezierZone>();

    private void OnEnable()
    {
        CurveTime = curveTime;
    }

    private void OnDisable()
    {
        inZones.Clear();
    }

    public bool IsOnCurve(BezierCurve curve)
    {
        return curve == this.curve;
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
            transform.position = curve.GetGlobalPoint(curveTime);
            if (curveTime == 0 || curveTime == 1)
            {
                BroadcastMessage("OnBezierEnd", this, SendMessageOptions.DontRequireReceiver);
            }

            BezierZone.PointIsInZones(this, tmpZones);
            for(int i=0, l=inZones.Count; i<l;i++)
            {
                if (!tmpZones.Contains(inZones[i]))
                {
                    BroadcastMessage("OnBezierZoneExit", inZones[i], SendMessageOptions.DontRequireReceiver);
                }
            }

            for (int i=0, l=tmpZones.Count; i<l; i++)
            {
                if (inZones.Contains(tmpZones[i]))
                {
                    BroadcastMessage("OnBezierZoneStay", tmpZones[i], SendMessageOptions.DontRequireReceiver);
                } else
                {
                    BroadcastMessage("OnBezierZoneEnter", tmpZones[i], SendMessageOptions.DontRequireReceiver);
                }
            }
            inZones.Clear();
            inZones.AddRange(tmpZones);
        }
    }


    public void Snap()
    {
        if (curve != null)
        {
            transform.position = curve.GetGlobalPoint(curveTime);
        }
    }
}
