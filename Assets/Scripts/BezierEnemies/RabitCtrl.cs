using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabitCtrl : MonoBehaviour {

    Prancer prancer;

    DeathLife deathLife;

	// Use this for initialization
	void Start () {
        deathLife = GetComponent<DeathLife>();
        prancer = GetComponent<Prancer>();	
	}
	
	// Update is called once per frame
	void Update () {
        prancer.enabled = deathLife.alive;
	}
}
