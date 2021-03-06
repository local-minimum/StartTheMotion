﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopMotionTransition
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

        return m_targetSequencer != null && (FromAnyState && !animator.HasActiveAnimtion || m_Source == animator.ActiveName) && !IsTriggerDriven;
        
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

    public bool AutoFires
    {
        get
        {
            return !IsTriggerDriven;
        }
    }

    public virtual bool CanTrigger(StopMotionAnimator animator, string trigger)
    {
        m_targetSequencer = animator.GetSequenceByName(m_Target);
        //Debug.Log(string.Format("{0} {1} {2} {3} {4} {5} {6}", IsTriggerDriven, trigger, m_Trigger, FromAnyState, animator.ActiveName, transitionSource, m_targetSequencer));
        return IsTriggerDriven && trigger == m_Trigger && (FromAnyState || animator.ActiveName == transitionSource) && m_targetSequencer != null; 
    }


    public virtual void Execute(StopMotionAnimator animator, bool resetAnimation)
    {
        animator.Play(m_targetSequencer, resetAnimation);
    }
}
