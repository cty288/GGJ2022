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

    public Texture2D clicked;
    public Texture2D normal;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        Cursor.SetCursor(normal, hotSpot, cursorMode);
        dogHead = GameObject.FindWithTag("DogHead");
        Animator = dogHead.GetComponent<Animator>();
    }
    private void OnMouseDown()
    {

        Cursor.SetCursor(clicked, hotSpot, cursorMode);

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

    public void OnMouseUp()
    {
        Cursor.SetCursor(normal, hotSpot, cursorMode);
    }
}
