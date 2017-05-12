using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyCtrl : MonoBehaviour {

    DeathLife deathLife;

    [SerializeField]
    DriveMotion driver;

	void Start () {
        deathLife = GetComponent<DeathLife>();
        		
	}

    void Update () {
        if (driver.enabled != deathLife.alive)
        {
            if (deathLife.alive)
            {
                driver.enabled = deathLife.alive;
                var pt = deathLife.GetComponent<BezierPoint>();
                pt.Attach(pt.Curve, 0);
            }
        }
	}
}
