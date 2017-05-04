using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrivenDeathLife : MonoBehaviour {

    [SerializeField]
    DeathLife deathLife;

    [SerializeField]
    BezierCurve[] dropPlayerOnCurves;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidCollider(collision))
        {
            collision.SendMessage("ShowDeathLife", deathLife, SendMessageOptions.DontRequireReceiver);            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!deathLife.CanCarry() && IsValidCollider(collision))
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

    bool IsValidCollider(Collider2D collision)
    {
        return deathLife && collision.GetComponent<CharacterCtrlr>() != null;
    }
}
