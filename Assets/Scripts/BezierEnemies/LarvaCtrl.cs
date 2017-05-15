using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaCtrl : MonoBehaviour {

    [SerializeField]
    CharacterCtrlr charCtrl;

    [SerializeField]
    CameraAttractor camAttractor;

    BezierPoint pt;

	void Start () {
        pt = GetComponent<BezierPoint>();
        camAttractor.isTracked = transform;
        charCtrl.canMove = false;
        pt.Attach(pt.Curve, pt.CurveTime);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
