using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonFadeOut : MonoBehaviour
{
    public void FadeOut()
    {
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.DOFade(0, 0.9f).OnComplete(() => Destroy(gameObject));
    }
}
