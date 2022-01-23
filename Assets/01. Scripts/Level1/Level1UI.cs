using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level1UI : MonoBehaviour {
    [SerializeField] private TMP_Text punchNumText;
    [SerializeField] private Image leftDirection;
    [SerializeField] private TMP_Text rightDirection;

    private void Start() {
        Level12Manager.Singleton.RemainingFightNum.RegisterWithInitValue(OnRemainingFightNumChange)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space)) {
            leftDirection.DOFade(0, 1);
           
        }

        if (Input.GetMouseButtonDown(0)) {
            rightDirection.DOFade(0, 1);
        }
    }

    private void OnRemainingFightNumChange(int oldNum, int newNum) {
        punchNumText.text = newNum.ToString();
    }
}
