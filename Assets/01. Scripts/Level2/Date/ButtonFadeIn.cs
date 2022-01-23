using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonFadeIn : MonoBehaviour
{
    public GameObject linkedButton;
    private void Start()
    {
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.DOFade(1, 2f);
    }
    
}
