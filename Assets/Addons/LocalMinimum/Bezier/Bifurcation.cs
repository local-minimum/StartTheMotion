using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bifurcation : MonoBehaviour {

    [SerializeField]
    BezierZoneEvent forkCondition;

    [SerializeField]
    BezierCurve fork;

    [SerializeField]
    float forkAttachTime;

    public System.Func<bool> externalCondtion;

    void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (externalCondtion == null || externalCondtion())
        {
            if (bEvent.type == forkCondition.type && bEvent.zone == forkCondition.zone)
            {
                if (forkCondition.point == null || forkCondition.point == bEvent.point)
                {
                    bEvent.point.ReAttach(fork, forkAttachTime);
                }
            }

        }

    }



}
