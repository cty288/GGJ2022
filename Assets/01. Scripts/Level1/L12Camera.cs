using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using UnityEngine;

public class L12Camera : MonoBehaviour {
    [SerializeField] private float lerpTime = 1f;
    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreeSpawned>(OnTreeSpawned).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnTreeSpawned(OnTreeSpawned e) {
        transform.DOMoveX(e.SpawnPosition.x, lerpTime);
    }
}
