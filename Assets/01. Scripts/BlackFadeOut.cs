using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class BlackFadeOut : MonoBehaviour
{
    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnLeftPrepareToStart>(StartFadeout)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void StartFadeout(OnLeftPrepareToStart obj) {
        Image image = GetComponent<Image>();

        Timer.Singleton.AddDelayTask(3, () => {
            image.DOFade(0, 1);
        });
        
    }
}
