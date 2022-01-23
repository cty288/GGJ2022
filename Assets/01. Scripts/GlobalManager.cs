using System.Collections;
using System.Collections.Generic;
using MikroFramework.BindableProperty;
using MikroFramework.Singletons;
using UnityEngine;

public class GlobalManager : MonoPersistentMikroSingleton<GlobalManager> {

    public BindableProperty<int> CurrentLevelNum = new BindableProperty<int>(0);
    public BindableProperty<int> Health = new BindableProperty<int>(3);


}
