using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    private void Start() {
        Timer.Singleton.AddDelayTask(10, () => {
            TypeEventSystem.SendGlobalEvent<OnLeftPrepareToStart>();
            Timer.Singleton.AddDelayTask(4, () => {
                TypeEventSystem.SendGlobalEvent<OnLeftStart>();
            });
        });
    }
}
