using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopMotionStart : IStopMotionTransition
{
    [SerializeField]
    string target;

    public string transitionSource
    {
        get
        {
            return null;
        }
    }

    public string transitionTarget
    {
        get
        {
            return target;
        }
    }

    public bool AutoFires
    {
        get
        {
            return true;
        }
    }

    StopMotionSequencer targetSequence; 
    public bool CanExecute(StopMotionAnimator animator)
    {
        targetSequence = animator.GetSequenceByName(target);
        return targetSequence != null;
    }

    public bool CanTrigger(StopMotionAnimator animator, string trigger)
    {
        return false;
    }

    public void Execute(StopMotionAnimator animator)
    {
        animator.Play(targetSequence);
    }
    
}
