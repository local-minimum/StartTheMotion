using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityAnimTrigger : MonoBehaviour {

    [SerializeField]
    string trigger;

    [SerializeField, Range(0, 1)]
    float probPerSecond;

    [SerializeField]
    string requiredState;

    StopMotionAnimator smAnim;

    private void Start()
    {
        smAnim = GetComponent<StopMotionAnimator>();
    }

    private void Update()
    {
        if (Random.value < probPerSecond * Time.deltaTime)
        {
            if (string.IsNullOrEmpty(requiredState) || smAnim.ActiveName == requiredState)
            {
                smAnim.Trigger(trigger);
            }        
        }
    }
}
