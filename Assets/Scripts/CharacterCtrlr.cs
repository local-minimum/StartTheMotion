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

    bool swappedCurveThisFrame;

    private const float noMove = 0.1f;
    void Update () {

        swappedCurveThisFrame = false;

        float hor = playerControlled ? Input.GetAxis("Horizontal") : 0f;
        if (hor > noMove)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
            point.Move(Time.deltaTime * speed * hor);
        }
        else if (hor < -noMove)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
            point.Move(Time.deltaTime * speed * hor);
        }

        if (!swappedCurveThisFrame && changePaths != null && Input.GetButtonDown("Fire1"))
        {
            var target = changePaths.GetTarget<BezierCurve>();
            Debug.Log("Swap to: " + target);
            if (target)
            {
                swappedCurveThisFrame = true;
                float t = target.TimeClosestTo(transform.position);
                if (t == 0)
                {
                    t = Mathf.Epsilon;
                } else if (t == 1)
                {
                    t = 1 - Mathf.Epsilon;
                }
                point.SwapAnchor(target, t);
                
            }
        }
	}
    
    BezierZone changePaths;

    bool playerControlled = true;

    void OnPointDriven(DriveMotion driveMotion)
    {
        playerControlled = false;
    }

    void KillPlayer(PointInvoker invoker)
    {
        Debug.Log("Respawn");
    }
}
