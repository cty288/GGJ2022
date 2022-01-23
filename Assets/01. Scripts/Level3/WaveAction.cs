using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using MikroFramework.Event;
using DG.Tweening;
using System;


/*
因为协商问题，做出如下：
forward -> hand
backward -> lie
 */

public struct CorrectAction
{

}

public class WaveAction : MonoBehaviour
{
    [SerializeField] private float verticalThredhold = 50;
    [SerializeField] private float horizontalThredhold = 50;

    [Serializable]
    public class WaveData
    {
        public float freq = 5f;
        public float objectScaleTime = .7f;
        public float animationTime = 1f;
        public float rollClearTime = 1f;
        public float delta = .25f;
    }
    public WaveData waveData;

    public Sprite[] sprites;

    [Serializable]
    public class WaveObject
    {
        public GameObject mesBox;
        public SpriteRenderer mesBoxContent;
        public SpriteRenderer dogSp;
        public SpriteRenderer tutorial;
        public Sprite[] rollStates;
        public Sprite[] handAndLie;
    }
    public WaveObject waveObject;

    float time;
    int currentCount;
    int currentAction;
    bool currentFinish;
    //0 = Forward
    //1 = Backward
    //2 = Roll

    Vector2 lastMousePosition;

    int lftCount, rtCount;
    bool lastAddLeft;

    void Start()
    {
        time = 0;
        currentCount = 1;
        currentAction = 114514;
        lftCount = 0;
        rtCount = 0;
    }

    void Update()
    {
        //ShakeCamera.Shake(true, 10);
        if (waveObject.tutorial.color.a == 1)
        {
            if (Input.GetMouseButtonDown(0)) waveObject.tutorial.DOFade(0, 2f);
        }
        time += Time.deltaTime;
        if(time > waveData.freq * currentCount)
         {
            currentCount++;
            currentFinish = false;
            currentAction = UnityEngine.Random.Range(0, 3);
            if (currentAction == 3) currentAction = 2;
            waveObject.mesBoxContent.sprite = sprites[currentAction];
            if (currentAction == 2) PlayClip(6); else PlayClip(5);
            ShowMesBox();
        }

        if (Input.GetMouseButton(0) && currentCount > 1)
        {
            Vector2 spd = (Vector2)Input.mousePosition - lastMousePosition;

            if(spd != Vector2.zero)
            {

                if (Math.Abs(spd.x) > Math.Abs(spd.y) && Mathf.Abs
                    (spd.x) >=horizontalThredhold)
                {
                    //水平

                    if (lftCount == 0 && rtCount == 0)
                    {
                        //Debug.Log("roll init");
                        lastAddLeft = true;
                        if (spd.x > 0)
                        {
                            rtCount++;
                            lastAddLeft = false;
                        }
                        else lftCount++;
                        Invoke("ResetRollCount", waveData.rollClearTime);
                    }
                    else
                    {
                        if (lastAddLeft)
                        {
                            if (spd.x > 0)
                            {
                                rtCount++;
                                lastAddLeft = false;
                            }
                        }
                        else
                        {
                            if (spd.x < 0)
                            {
                                lftCount++;
                                lastAddLeft = true;
                            }
                        }
                    }

                    if (lftCount > 1 && rtCount > 1)
                    {
                        CancelInvoke("ResetRollCount");
                        ResetRollCount();
                        PlayDogAnimation(2);
                    }
                }
                if (Math.Abs(spd.y) > Math.Abs(spd.x) && Mathf.Abs(spd.y) >= verticalThredhold)
                {
                    //竖直
                    int idx = (spd.y > 0) ? 0 : 1;
                    PlayDogAnimation(idx);
                }
            }
        }

        lastMousePosition = Input.mousePosition;

    }

    void ResetRollCount()
    {
        lftCount = 0;
        rtCount = 0;
    }

    float lastAniPlayTime = -114514;
    //0 = Forward
    //1 = Backward
    //2 = Roll
    void PlayDogAnimation(int index)
    {
        if (time - lastAniPlayTime > waveData.animationTime)
        {
            lastAniPlayTime = time;
            DogAnimator(index);
            Debug.Log("Anim: " +index);
            if(index == currentAction)
            {
                CorrectAction();
            } else
            {
                ShakeCamera.Shake(false, 5,35,0.3f);
            }
        }
    }

    int curIndex;

    void DogAnimator(int index)
    {
        //狗的动画&音效
        if (UnityEngine.Random.Range(0, 1f) > .5f) PlayClip(1); else PlayClip(2);
        if (index == 2)
        {
            curIndex = 0;
            DogRoll();
        }
        else
        {
            waveObject.dogSp.sprite = waveObject.handAndLie[index];
            Invoke("ResumeDog", waveData.animationTime);
        }
    }

    void ResumeDog()
    {
        waveObject.dogSp.sprite = waveObject.rollStates[3];
    }

    void DogRoll()
    {
        Debug.Log("RRRR");
        waveObject.dogSp.sprite = waveObject.rollStates[curIndex];
        curIndex++;
        if (curIndex == 4) return;
        Invoke("DogRoll", waveData.delta);
    }

    [SerializeField] private AudioClip successAudio;
    void CorrectAction()
    {
        if (currentFinish) return;
        SendCorrectAction();
        Debug.Log("OHHHHHHHH");
        currentFinish = true;
        //有“成功执行动作”的音效就播放吧
        AudioManager.Singleton.PlayAudioShot(successAudio,1f);
    }

    void ShowMesBox()
    {
        waveObject.mesBox.transform.DOScale(new Vector3(1, 1, 1), waveData.objectScaleTime);
        Invoke("HideMesBox", waveData.freq - waveData.objectScaleTime);
    }

    void HideMesBox()
    {
        waveObject.mesBox.transform.DOScale(new Vector3(0, 0, 0), waveData.objectScaleTime);
    }

    void SendCorrectAction()
    {
        CorrectAction correctAction = new CorrectAction();
        TypeEventSystem.SendGlobalEvent(correctAction);
    }

    void PlayClip(int index)
    {
        PlayAudio playAudio = new PlayAudio();
        playAudio.index = index;
        TypeEventSystem.SendGlobalEvent(playAudio);
    }
}
