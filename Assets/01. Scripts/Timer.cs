using System;
using System.Collections;
using System.Collections.Generic;

using MikroFramework.Singletons;
using MikroFramework.TimeSystem;
using UnityEngine;

public class Timer : MikroSingleton<Timer> {
    private TimeSystem timeSystem;

    public override void OnSingletonInit() {
        base.OnSingletonInit();
        timeSystem = new TimeSystem();
        timeSystem.Start();
    }

    public void AddDelayTask(float second, Action delayAction) {
        timeSystem.AddDelayTask(second, delayAction);
    }

    public void Pause() {
        timeSystem.Pause();
    }

    public void Resume() {
        timeSystem.Start();
    }
}
