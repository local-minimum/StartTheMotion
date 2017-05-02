using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndToEndTrigger : MonoBehaviour
{
    [SerializeField]
    BezierCurve zeroTime;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float zeroTimeTime = .99999f;

    [SerializeField]
    BezierCurve oneTime;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float oneTimeTime = 0.00001f;

    private void Reset()
    {
        if (transform.parent)
        {
            zeroTime = transform.parent.GetComponentInParent<BezierCurve>();
        }
        foreach (BezierCurve c in GetComponentsInChildren<BezierCurve>()) {
            if (c.gameObject != gameObject)
            {
                oneTime = c;
                break;
            }
        }
    }

    void OnBezierEnd(BezierPoint point)
    {
        if (point.CurveTime == 0 && zeroTime)
        {
            point.ReAttach(zeroTime, zeroTimeTime);
        } else if (point.CurveTime == 1 && oneTime)
        {
            point.ReAttach(oneTime, oneTimeTime);
        }

        
    }
}
