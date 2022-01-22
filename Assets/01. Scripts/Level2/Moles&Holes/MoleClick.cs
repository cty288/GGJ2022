using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.BindableProperty;
using MikroFramework.Event;

public class MoleClick : MonoBehaviour
{

    private void Awake()
    {
    }
    private void OnMouseDown(){
        MolesManager.count++;
        Destroy(this.gameObject);

        DateManager.Singleton.DestroyHint();
    }
}
