using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopMotionTransition : AbstractStopMotionTransition
{
    [SerializeField]
    string m_Source;

    public override string transitionSource
    {
        get
        {
            return m_Source;
        }
    }

    public bool FromAnyState
    {
        get
        {
            return string.IsNullOrEmpty(m_Source);
        }
    }

    [SerializeField]
    string m_Target;

    public override string transitionTarget
    {
        get
        {
            return m_Target;
        }
    }

    StopMotionSequencer m_targetSequencer;

    public override bool CanExecute(StopMotionAnimator animator)
    {
        m_targetSequencer = animator.GetSequenceByName(m_Target);

        return m_targetSequencer != null && (FromAnyState || m_Source == animator.ActiveName);
        
    }

    [SerializeField]
    string m_Trigger;

    public string Trigger
    {
        get
        {
            return m_Trigger;
        }
    }

    public bool IsTriggerDriven
    {
        get
        {
            return !string.IsNullOrEmpty(m_Trigger);
        }
    }

    public override bool AutoFires
    {
        get
        {
            return !IsTriggerDriven;
        }
    }

    public override bool CanTrigger(StopMotionAnimator animator, string trigger)
    {
        return IsTriggerDriven && trigger == m_Trigger && CanExecute(animator); 
    }


    public override void Execute(StopMotionAnimator animator)
    {
        animator.Play(m_targetSequencer);
    }
}
