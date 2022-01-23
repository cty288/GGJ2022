using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using UnityEngine;

public class ShootPee : MonoBehaviour
{
    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreePass).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnTreePass(OnTreePassed e) {
        GameObject pee= Instantiate(e.PeeTree.treeShootAnimationPrefab, transform);
        //pee.AddComponent<RectTransform>();
        pee.transform.localPosition = Vector3.zero;
        Destroy(pee, 2f);
    }
}
