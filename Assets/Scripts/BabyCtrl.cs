using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCtrl : MonoBehaviour {

    [SerializeField]
    Transform holdingHand;

    [SerializeField]
    Transform selfRoot;

    [SerializeField]
    Transform selfHand;

    [SerializeField, Range(0, 30)]
    float snappiness = 10f;

    [SerializeField, Range(0, 4)]
    float animationProb = 0.1f;

    StopMotionAnimator stAnim;

	// Use this for initialization
	void Start () {
        stAnim = GetComponent<StopMotionAnimator>();	
	}
	
	// Update is called once per frame
	void Update () {

	    if (Random.value < animationProb * Time.deltaTime)
        {
            if (stAnim.ActiveName != "Bounce")
            {
                stAnim.ActivateByName("Bounce");
            }
            stAnim.StepToRandomInActive();
        }

        Vector3 offset = holdingHand.position - selfHand.position;
        selfRoot.position += offset.normalized * Mathf.Lerp(0, offset.magnitude, snappiness * Time.deltaTime);
	}
}
