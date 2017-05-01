using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopMotionTransition : IStopMotionTransition
{
    [SerializeField]
    string m_Source;

    public string transitionSource
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

    public string transitionTarget
    {
        get
        {
            return m_Target;
        }
    }

    StopMotionSequencer m_targetSequencer;

    public bool CanExecute(StopMotionAnimator animator)
    {
        m_targetSequencer = animator.GetSequenceByName(m_Target);

        return m_targetSequencer != null && (FromAnyState || m_Source == animator.ActiveName);
        
    }

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

    public bool AutoFires
    {
        get
        {
            return !IsTriggerDriven;
        }
    }

    public bool CanTrigger(StopMotionAnimator animator, string trigger)
    {
        return IsTriggerDriven && trigger == m_Trigger && CanExecute(animator); 
    }


    public void Execute(StopMotionAnimator animator)
    {
        animator.Play(m_targetSequencer);
    }
}
