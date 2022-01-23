using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextFadeIn : MonoBehaviour
{
    public TMP_Text text;
    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(5f);
        text.DOFade(0, 1);
    }
}
