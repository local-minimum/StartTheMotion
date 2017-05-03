using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndToEndTrigger : MonoBehaviour
{
    [SerializeField]
    BezierCurve leftTransitionTo;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float leftTransitionToTime = .99999f;

    [SerializeField]
    BezierCurve rightTransitionTo;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float rightTransitionToTime = 0.00001f;

    private void Reset()
    {
        if (transform.parent)
        {
            leftTransitionTo = transform.parent.GetComponentInParent<BezierCurve>();
        }
        foreach (BezierCurve c in GetComponentsInChildren<BezierCurve>()) {
            if (c.gameObject != gameObject)
            {
                rightTransitionTo = c;
                break;
            }
        }
    }

    void OnBezierEnd(BezierPoint point)
    {
        if (point.CurveTime == 0 && leftTransitionTo)
        {
            point.ReAttach(leftTransitionTo, leftTransitionToTime);
        } else if (point.CurveTime == 1 && rightTransitionTo)
        {
            point.ReAttach(rightTransitionTo, rightTransitionToTime);
        }

        
    }
}
