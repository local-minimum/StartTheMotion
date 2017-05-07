using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPinger : MonoBehaviour {

    [SerializeField]
    CharacterCtrlr syncCtrl;

    [SerializeField]
    BezierPoint coordinatingPoint;

    bool charOnLine;

	void Update () {

		for (int i=0, l=pingers.Count; i< l; i++)
        {
            pingers[i].Snap();
        }

        if (syncCtrl && charOnLine)
        {
            syncCtrl.SetMovementConfig(coordinatingPoint.GetDirection());
        }
	}

    List<BezierPoint> pingers = new List<BezierPoint>();

    void OnAttachToCurve(BezierPoint pt)
    {
        if (!pingers.Contains(pt))
        {
            pingers.Add(pt);
            charOnLine = true;
        }
    }

    void OnDetachFromCurve(BezierPoint pt)
    {
        pingers.Remove(pt);
        charOnLine = false;
        syncCtrl.SetMovementConfig(MoveDirection.Right);
    }
}
