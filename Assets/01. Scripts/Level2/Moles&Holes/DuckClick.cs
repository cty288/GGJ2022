using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DuckClick : MonoBehaviour
{
    public int id = -1;
    [SerializeField] private GameObject dogHead;
    [SerializeField] private Animator Animator;
    [SerializeField] private AudioClip clickDuck;

    private void Awake()
    {
        dogHead = GameObject.FindWithTag("DogHead");
        Animator = dogHead.GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        AudioManager.Singleton.PlayAudioShot(clickDuck, 1);
        if (!DateManager.Singleton.isShaking)
        {
            DateManager.Singleton.isShaking = true;
            ShakeCamera.Shake(false, 2, 20);
            DateManager.Singleton.isShaking = false;
        }

        MolesManager.count++;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();

        DOTween.To(() => render.color, value => render.color = value, new Color(render.color.r, render.color.g, render.color.b, 0),
            .7f).SetEase(Ease.InBounce).OnComplete(() => Destroy(gameObject));

        MolesManager.Singleton.occupiedHoleList.Remove(this.id);
        Animator.SetInteger("State", 1);
    }
}
