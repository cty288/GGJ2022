using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class ThinkImage : MonoBehaviour
{
    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnLeftPrepareToStart>(OnLeftPrepareToStart)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnLeftPrepareToStart(OnLeftPrepareToStart obj) {
        GetComponent<Animation>().Play();
    }
}
