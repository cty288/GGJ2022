using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DuckAutoDisappear : MonoBehaviour
{
    public float disappearTime = 5f;
    private void Start()
    {
        StartCoroutine(DisappearCountDown(disappearTime));
    }

    public IEnumerator DisappearCountDown (float disappearTime)
    {
        yield return new WaitForSeconds(disappearTime);
        float currTime = 0;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();

        DOTween.To(() => render.color, value => render.color = value, new Color(render.color.r, render.color.g, render.color.b, 0),
            disappearTime).SetEase(Ease.InBounce).OnComplete(()=> Destroy(gameObject));
    }
}
