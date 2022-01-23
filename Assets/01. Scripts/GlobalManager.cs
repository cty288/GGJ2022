using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.BindableProperty;
using MikroFramework.Event;
using MikroFramework.Singletons;
using UnityEngine;

public struct OnGameover {

}

public struct OnLevelPass {

}
public class GlobalManager : MonoPersistentMikroSingleton<GlobalManager> {

    public BindableProperty<int> CurrentLevelNum = new BindableProperty<int>(0);
    public BindableProperty<int> Health = new BindableProperty<int>(3);

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnPlayerFail>(OnPlayerFail).UnRegisterWhenGameObjectDestroyed(gameObject);

    }

    private void OnPlayerFail(OnPlayerFail obj) {
        Health.Value--;
        if (Health.Value <= 0) {
            TypeEventSystem.SendGlobalEvent<OnGameover>();
        }
    }

    public void ResetHealth() {
        Health.Value = 3;
    }

}
