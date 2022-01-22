using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class PeeTreeSlider : MonoBehaviour {
    private Slider peeTreeSlider;
    [SerializeField] private GameObject TriggerArea;
    [SerializeField] private float sliderSpeed = 0.4f;
    [SerializeField] private float sliderWidth = 0.1f;

    [SerializeField]
    private bool sliderMove = false;
    private bool sliderMoveForward = false;
    private float peeTreeChargePercentage = 0;

    private void Awake() {
        peeTreeSlider = GetComponent<Slider>();
        TriggerArea.transform.localScale = new Vector3(sliderWidth, 1, 1);
    }

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreeSpawned>(OnTreeSpawned).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void Update() {
        if (sliderMove) {
            if (Input.GetKey(KeyCode.Space)) {
                peeTreeSlider.value += (sliderMoveForward ? 1 : -1) * sliderSpeed * Time.deltaTime;
                if (sliderMoveForward && peeTreeSlider.value >= 1)
                {
                    sliderMoveForward = false;
                }

                if (!sliderMoveForward && peeTreeSlider.value <= 0)
                {
                    sliderMoveForward = true;
                }

            }

            if (Input.GetKeyUp(KeyCode.Space)) {
               
                if (peeTreeSlider.value >= peeTreeChargePercentage &&
                    peeTreeSlider.value <= peeTreeChargePercentage + sliderWidth) {
                    TypeEventSystem.SendGlobalEvent<OnTreePassed>();
                    //sliderMove = false;
                }
                else {
                    peeTreeSlider.value = 0;
                    sliderMoveForward = true;
                    TypeEventSystem.SendGlobalEvent<OnPlayerFail>();
                    Debug.Log("Fail");
                }
            }
        }
    }

    private void OnTreeSpawned(OnTreeSpawned e) {
        float width = GetComponent<RectTransform>().sizeDelta.x;
        TriggerArea.transform.localPosition = new Vector2(-width / 2 + width * e.PeeTree.PeeChargePosition, 0);
        sliderMove = true;
        peeTreeChargePercentage = e.PeeTree.PeeChargePosition;
        sliderMoveForward = true;
        peeTreeSlider.value = 0;
    }
}
