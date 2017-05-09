using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCtrl : MonoBehaviour {

    [SerializeField]
    Transform theirHand;

    [SerializeField]
    Transform selfRoot;

    [SerializeField]
    Transform selfHand;

    [SerializeField, Range(0, 30)]
    float snappiness = 10f;

    [SerializeField, Range(0, 10)]
    float animationProb = 0.1f;

    StopMotionAnimator stAnim;

    StopMotionAnimator theirStAnim;

    Vector3 localScale;
	// Use this for initialization
	void Start () {
        localScale = transform.localScale;
        stAnim = GetComponent<StopMotionAnimator>();
        theirStAnim = theirHand.GetComponentInParent<StopMotionAnimator>();
	}
	
	// Update is called once per frame
    bool TheyAllowBounce
    {
        get
        {
            return !(string.IsNullOrEmpty(theirStAnim.ActiveName) || theirStAnim.ActiveName.StartsWith("Idle"));
        }
    }

    float transformXDirection
    {
        get
        {
            return !TheyAllowBounce ? Mathf.Sign(theirStAnim.transform.localScale.x) : -1f;
        }
    }

	void Update () {

	    if (TheyAllowBounce && Random.value < animationProb * Time.deltaTime)
        {
            if (stAnim.ActiveName != "Bounce")
            {
                stAnim.ActivateByName("Bounce");
            }
            stAnim.StepToRandomInActive();
        }

        Vector3 offset = theirHand.position - selfHand.position;
        selfRoot.position += offset.normalized * Mathf.Lerp(0, offset.magnitude, snappiness * Time.deltaTime);
        localScale.x = Mathf.Abs(localScale.x) * transformXDirection;
        transform.localScale = localScale;
	}
}
