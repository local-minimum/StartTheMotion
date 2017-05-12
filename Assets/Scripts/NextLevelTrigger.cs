using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour {

    [SerializeField]
    BezierZone loadZone;

    [SerializeField]
    BezierZoneEventType transportOn;

    [SerializeField]
    string nextName;

    [SerializeField]
    BezierPoint point;

    private void Start()
    {
        loadZone.ForwardTo(this);
    }


}
