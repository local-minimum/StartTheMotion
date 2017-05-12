using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrivenDeathLife : MonoBehaviour {

    [SerializeField]
    DeathLife deathLife;

    [SerializeField]
    BezierCurve[] dropPlayerOnCurves;

    [SerializeField]
    public bool allowKilling = true;

    public bool allowLifing = true;

    [SerializeField]
    BezierCurve[] requireCurves;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(string.Format("{0} {1} {2} {3}", this, LifeKillAllowed(), IsValidCollider(collision), isValidCurve(collision)));
        if (LifeKillAllowed() && IsValidCollider(collision) && isValidCurve(collision))
        {
            collision.SendMessage("ShowDeathLife", deathLife, SendMessageOptions.DontRequireReceiver);            
        }
    }

    bool LifeKillAllowed()
    {
        if (deathLife.alive)
        {
            return allowKilling;
        } else
        {
            return allowLifing;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!deathLife.CanCarry() && IsValidCollider(collision) && isValidCurve(collision))
        {
            BezierPoint bPt = collision.GetComponent<BezierPoint>();
            if (bPt) {
                for (int i = 0; i<dropPlayerOnCurves.Length; i++) {
                    if (dropPlayerOnCurves[i] == bPt.Curve)
                    {
                        collision.SendMessage("PointDrop", deathLife);
                        return;
                    }
                
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidCollider(collision))
        {
            collision.SendMessage("HideDeathLife", deathLife, SendMessageOptions.DontRequireReceiver);
        }
    }

    public bool IsValidCollider(Collider2D collision)
    {
        return deathLife && collision.GetComponent<CharacterCtrlr>() != null;
    }

    bool isValidCurve(Collider2D collision)
    {
        if (requireCurves.Length > 0)
        {
            BezierPoint pt = collision.GetComponent<BezierPoint>();
            for (int i=0, l=requireCurves.Length; i<l; i++)
            {
                Debug.Log(string.Format("{0}.{1} vs {2}", pt, pt.Curve, requireCurves[i]));
                if (pt.Curve == requireCurves[i])
                {
                    
                    return true;
                }
            }
            return false;

        }
        return true;
    }
}
