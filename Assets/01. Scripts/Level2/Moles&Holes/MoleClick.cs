using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.BindableProperty;
using MikroFramework.Event;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MoleClick : MonoBehaviour
{
    public int id = -1;
    [SerializeField] private GameObject dogHead;
    [SerializeField] private Animator Animator;
    [SerializeField] private AudioClip moleClick1;
    [SerializeField] private AudioClip moleClick2;
    [SerializeField] private AudioClip moleClick3;

    public Texture2D clicked;
    public Texture2D normal;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    private void Awake()
    {
        dogHead = GameObject.FindWithTag("DogHead");
        Animator = dogHead.GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void OnMouseDown(){

        Cursor.SetCursor(clicked, hotSpot, cursorMode);

        if (MolesManager.Singleton.audioIndex == 0)
        {
            AudioManager.Singleton.PlayAudioShot(moleClick1, 1);
            MolesManager.Singleton.audioIndex++;
        }
        else if (MolesManager.Singleton.audioIndex == 1)
        {
            AudioManager.Singleton.PlayAudioShot(moleClick2, 1);
            MolesManager.Singleton.audioIndex++;
        }
        else if (MolesManager.Singleton.audioIndex == 2)
        {
            AudioManager.Singleton.PlayAudioShot(moleClick3, 1);
            MolesManager.Singleton.audioIndex = 0;
        }

        Debug.Log(dogHead);
        MolesManager.count++;
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.DOFade(0, 1f);//OnComplete(() => Destroy(gameObject));
        
        Debug.Log("233");
        DateManager.Singleton.DestroyHint();
        MolesManager.Singleton.occupiedHoleList.Remove(this.id);
        Animator.SetInteger("State", 0);
    }

    public void OnMouseUp()
    {
        Cursor.SetCursor(normal, hotSpot, cursorMode);
    }
}
