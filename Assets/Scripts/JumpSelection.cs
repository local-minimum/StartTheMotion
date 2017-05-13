using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSelection : MonoBehaviour {

    [SerializeField]
    JumpAlternative[] alternativePriority;

    [SerializeField]
    CharacterCtrlr player;

    BezierPoint pt;

    JumpAlternative GetJump()
    {
        for (int i=0, l=alternativePriority.Length; i< l; i++)
        {
            if (alternativePriority[i].CanJump)
            {
                return alternativePriority[i];
            }
        }

        return null;
    }

    void Start()
    {
        pt = player.GetComponent<BezierPoint>();
    }

    void Update()
    {
        if (player.IsInControl && Input.GetButtonDown("Jump") && player.CanJump && InJumpZone(player.GetComponent<BezierPoint>()))
        {
            Debug.Log(string.Format("Player is on {0}={1} in zone {2} thus can jump", 
                player.GetComponent<BezierPoint>().Curve, zone.curve, 
                InJumpZone(player.GetComponent<BezierPoint>())));
            var jump = GetJump();
            if (jump != null)
            {
                jump.Grab(pt);
                player.GetComponent<StopMotionAnimator>().Trigger("Jump");
            }
        } 
    }

    public bool InJumpZone(BezierPoint pt)
    {
        if (zone)
        {
            return zone.IsInside(pt.Curve, pt.CurveTime);
        }

        return false;
    }

    BezierZone zone;

    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        zone = bEvent.zone;
        if (bEvent.type == BezierZoneEventType.EnterZone)
        {
            var player = bEvent.point.GetComponent<CharacterCtrlr>();
            if (player)
            {
                player.jumpIcon.Show();
            }
        } else if (bEvent.type == BezierZoneEventType.ExitZone)
        {
            var player = bEvent.point.GetComponent<CharacterCtrlr>();
            if (player)
            {
                //player.jumpIcon.Hide();
            }
        }
    }
}
