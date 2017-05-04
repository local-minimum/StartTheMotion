using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrivenDeathLife : MonoBehaviour {

    [SerializeField]
    DeathLife deathLife;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidCollider(collision))
        {
            collision.SendMessage("ShowDeathLife", deathLife, SendMessageOptions.DontRequireReceiver);            
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
