using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bifurcation : MonoBehaviour {

    [SerializeField]
    BezierZoneEvent forkCondition;

    [SerializeField]
    BezierCurve fork;

    [SerializeField, Range(0.00001f, 0.99999f)]
    float forkAttachTime = 0.00001f;

    public System.Func<bool> externalCondtion;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (bEvent.type == forkCondition.type && bEvent.zone == forkCondition.zone)
        {
            if (externalCondtion == null || externalCondtion())
            {
                if (forkCondition.point == null || forkCondition.point == bEvent.point)
                {
                    Debug.Log(string.Format(
                        "Attatch {0} from {1} to {2} at {3} time",
                        bEvent.point, bEvent.point.Curve, fork, forkAttachTime
                        ));
                    bEvent.point.Attach(fork, forkAttachTime);
                }
            }

        }

    }



}
