using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpIconer : MonoBehaviour {

    [SerializeField]
    Vector3 playerOffset;

    [SerializeField]
    Transform player;

    Color c;
    float maxAlfa;
    SpriteRenderer sRend;

    [SerializeField]
    AnimationCurve alphaTransition;

    [SerializeField]
    float showDuration = 2f;

    private void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        c = sRend.color;
        maxAlfa = c.a;
        c.a = 0;
        sRend.color = c;
    }
    bool showing;

    public void Show()
    {
        if (!showing)
        {
            StartCoroutine(_Show());
        }
    }

    public void Hide()
    {
        showing = false;
    }

    IEnumerator<WaitForSeconds> _Show()
    {
        showing = true;
        float start = Time.timeSinceLevelLoad;
        float progress = 0;
        do
        {
            progress = (Time.timeSinceLevelLoad - start) / showDuration;
            c.a = Mathf.Lerp(0, maxAlfa, alphaTransition.Evaluate(progress));
            sRend.color = c;
            transform.position = player.position + playerOffset;
            yield return new WaitForSeconds(0.016f);
        } while (progress < 1 && showing);
        c.a = 0;
        sRend.color = c;
        showing = false;
    }
}
