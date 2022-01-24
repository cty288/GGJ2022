using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartEndBG : MonoBehaviour {
    private Image bgImage;

    [SerializeField] private float time = 1f;
    private void Awake() {
        bgImage = GetComponent<Image>();
    }

    void Start() {
        GlobalManager.Singleton.Health.RegisterOnValueChaned(OnHealthChange)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        Timer.Singleton.AddDelayTask(1f, () => {
            bgImage.DOFade(0, 1);
        });

        TypeEventSystem.RegisterGlobalEvent<OnLevelPass>(OnLevelPass).UnRegisterWhenGameObjectDestroyed(gameObject);


    }

    private void OnLevelPass(OnLevelPass obj) {
        bgImage.DOFade(1, time).OnComplete(() => {
            GlobalManager.Singleton.ResetHealth();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        });
    }

    private void OnHealthChange(int arg1, int newHealth) {
        if (newHealth == 0) {
            bgImage.DOFade(1, time).OnComplete(() => {
                GlobalManager.Singleton.ResetHealth();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
    }

   
}
