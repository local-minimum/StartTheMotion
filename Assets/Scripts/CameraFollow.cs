using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    [SerializeField]
    CameraAttractor attractor;

    [SerializeField]
    float nice = 2f;

    [SerializeField]
    float attack = 0.1f;

    [SerializeField]
    float evil = 10f;

    [SerializeField]
    float fast = 0.5f;

    void Update () {
        float sq = Vector3.SqrMagnitude(attractor.transform.position - transform.position);

        if (sq < nice)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, attractor.transform.position, Mathf.Lerp(attack, fast, sq / evil) * Time.deltaTime);
	}
}
