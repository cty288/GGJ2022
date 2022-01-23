using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using UnityEngine;

public struct OnLeftStart {

}

public struct OnLeftPrepareToStart {

}


public class GameStartController : MonoBehaviour {
    [SerializeField] private List<GameObject> objs;

    private void Awake() {
        objs.ForEach(obj => obj.SetActive(false));
    }

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnLeftStart>(OnLeftStart).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnLeftStart(OnLeftStart obj) {
        objs.ForEach(obj => obj.SetActive(true));
    }
}
