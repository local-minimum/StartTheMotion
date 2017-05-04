using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLife : MonoBehaviour {

    [SerializeField]
    bool _aliveOnAwake = true;

    [SerializeField]
    Bifurcation[] affectedBifurcations;

    [SerializeField]
    bool affectedBifAliveReq;

    bool _alive;

    public bool alive
    {
        get
        {
            return _alive;
        }
        set
        {
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i].Trigger(value ? "Life" : "Death");
            }
            _alive = value;
        }
    }

    [SerializeField]
    StopMotionAnimator[] animations;

    private void Start()
    {
        alive = _aliveOnAwake;
        for (int i=0; i<affectedBifurcations.Length; i++)
        {
            affectedBifurcations[i].externalCondtion = IsAlive;
        }
    }

    bool IsAlive()
    {
        return alive == affectedBifAliveReq;
    }

    [SerializeField]
    bool carryWhenAlive;

    public bool CanCarry()
    {
        return alive == carryWhenAlive;
    }
}
