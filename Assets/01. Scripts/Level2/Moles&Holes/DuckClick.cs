using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DuckClick : MonoBehaviour
{
    public int id = -1;
    [SerializeField] private GameObject dogHead;
    [SerializeField] private Animator Animator;

    private void Awake()
    {
        dogHead = GameObject.FindWithTag("DogHead");
        Animator = dogHead.GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        
        MolesManager.count++;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();

        DOTween.To(() => render.color, value => render.color = value, new Color(render.color.r, render.color.g, render.color.b, 0),
            .7f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));

        DateManager.Singleton.DestroyHint();
        MolesManager.Singleton.occupiedHoleList.Remove(this.id);
        Animator.SetInteger("State", 1);
    }
}
