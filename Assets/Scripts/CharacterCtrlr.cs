using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrlr : MonoBehaviour {

    BezierPoint point;

    public float speed = 2;

    private void Start()
    {
        point = GetComponent<BezierPoint>();
    }

    void Update () {
        float hor = Input.GetAxis("Horizontal");
        if (hor > 0.001f)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;

        } else if (hor < -0.001f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
        point.Move(Time.deltaTime * speed * hor);

        if (changePaths != null && Input.GetButtonDown("Fire1"))
        {
            var target = changePaths.GetTarget<BezierCurve>();
            Debug.Log("Swap to: " + target);
            if (target)
            {
                float t = target.TimeClosestTo(transform.position);
                point.SwapAnchor(target, t);
            }
        }
	}

    void OnBezierEnd(BezierPoint bPt)
    {
    }

    BezierZone changePaths;

    void OnBezierZoneEnter(BezierZone zone)
    {
        Debug.Log(zone);
        if (zone.GetTarget<BezierCurve>() != null)
        {
            Debug.Log("Enter " + zone);
            changePaths = zone;
        }
    }

    void OnBezierZoneExit(BezierZone zone)
    {
        if (changePaths == zone)
        {
            Debug.Log("Exit " + zone);
            changePaths = null;
        }
    }
}
