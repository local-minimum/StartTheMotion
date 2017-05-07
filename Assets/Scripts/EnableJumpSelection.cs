using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableJumpSelection : MonoBehaviour {

    [SerializeField]
    JumpSelection jSelect;

    
    void OnAttachToCurve(BezierPoint pt)
    {
        jSelect.enabled = true;
    }

    void OnDetachFromCurve(BezierPoint pt)
    {
        jSelect.enabled = false;
    }
}
