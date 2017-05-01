using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopMotionStart : AbstractStopMotionTransition
{
    [SerializeField]
    string target;

    public override string transitionSource
    {
        get
        {
            return null;
        }
    }

    public override string transitionTarget
    {
        get
        {
            return target;
        }
    }

    public override bool AutoFires
    {
        get
        {
            return true;
        }
    }

    StopMotionSequencer targetSequence; 
    public override bool CanExecute(StopMotionAnimator animator)
    {
        targetSequence = animator.GetSequenceByName(target);
        return targetSequence != null;
    }

    public override bool CanTrigger(StopMotionAnimator animator, string trigger)
    {
        return false;
    }

    public override void Execute(StopMotionAnimator animator)
    {
        animator.Play(targetSequence);
    }
    
}
