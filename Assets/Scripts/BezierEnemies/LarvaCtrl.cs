using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaCtrl : MonoBehaviour {

    [SerializeField]
    CharacterCtrlr charCtrl;

    [SerializeField]
    CameraAttractor camAttractor;

    [SerializeField]
    StopMotionSequencer invadSeq;

    [SerializeField]
    int invadedIndex = 22;

    BezierPoint pt;

    StopMotionAnimator smAnim;

	void Start () {
        smAnim = GetComponent<StopMotionAnimator>();
        pt = GetComponent<BezierPoint>();
        camAttractor.isTracked = transform;
        charCtrl.canMove = false;
        pt.Attach(pt.Curve, pt.CurveTime);
	}

    private void OnEnable()
    {
        invadSeq.OnSequenceFrame += InvadSeq_OnSequenceFrame;
    }

    private void OnDisable()
    {
        invadSeq.OnSequenceFrame -= InvadSeq_OnSequenceFrame;
    }

    private void InvadSeq_OnSequenceFrame(StopMotionSequencer sequencer)
    {
        if (sequencer.ShowingIndex == invadedIndex)
        {
            smAnim.Stop();
            charCtrl.canMove = true;
            camAttractor.isTracked = charCtrl.transform;
            Destroy(gameObject);
        }
    }

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        if (bEvent.point == pt && bEvent.type == BezierZoneEventType.EnterZone)
        {
            smAnim.Trigger("Invade");
        }
    }

}
