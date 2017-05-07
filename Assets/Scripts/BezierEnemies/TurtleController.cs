using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour {

    [SerializeField]
    StopMotionAnimator turtleAnim;

    [SerializeField]
    DeathLife water;

    [SerializeField]
    DeathLife turtle;

    [SerializeField]
    Prancer prancer;

    bool isKiller = true;

    TriggerDrivenDeathLife tddl;

    void Start()
    {
        tddl = GetComponent<TriggerDrivenDeathLife>();
    }

    void Update()
    {

        if (turtle.alive)
        {
            if (water.alive)
            {
                if (turtleAnim.ActiveName != "Happy")
                {
                    turtleAnim.Trigger("Happy");
                    isKiller = false;
                    prancer.enabled = true;
                    tddl.allowKilling = true;
                    transform.localRotation = Quaternion.identity;
                }
            }
            else
            {
                if (turtleAnim.ActiveName != "Angry")
                {
                    turtleAnim.Trigger("Angry");
                    isKiller = true;
                    prancer.enabled = false;
                    tddl.allowKilling = false;
                    transform.localRotation = Quaternion.Euler(0, 0, prancer.walkDirection * 60f);
                }
            }
        } else
        {
            prancer.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isKiller && tddl.IsValidCollider(collision))
        {
            collision.SendMessage("KillPlayer");
        }
    }
}
