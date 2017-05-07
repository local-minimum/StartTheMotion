using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSelection : MonoBehaviour {

    [SerializeField]
    JumpAlternative[] alternativePriority;

    bool inZone;

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
        if (inZone && player.IsInControl && Input.GetButtonDown("Jump"))
        {
            var jump = GetJump();
            if (jump != null)
            {
                jump.Grab(pt);   
            }
        } 
    }


    public void OnBezierZoneEvent(BezierZoneEvent bEvent)
    {
        inZone = bEvent.type != BezierZoneEventType.ExitZone;
    }
}
