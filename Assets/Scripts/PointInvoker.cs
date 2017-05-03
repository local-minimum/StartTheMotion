using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInvoker : MonoBehaviour {

    [SerializeField]
    string[] Actions;
    
    [HideInInspector]
    public bool abortActionChain;

    public void SendActions(BezierPoint point) {
        abortActionChain = false;
        for (int i=0; i<Actions.Length;i++)
        {
            point.SendMessage(Actions[i], this, SendMessageOptions.DontRequireReceiver);
            if (abortActionChain)
            {
                break;
            }
        }
    }
}
