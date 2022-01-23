using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using MikroFramework.Singletons;
using UnityEngine;


public enum AudioType {
    Happy,
    Soleum
}

public class AudioManager : MonoPersistentMikroSingleton<AudioManager> {
    [SerializeField] private AudioSource happyBGMSource;
    [SerializeField] private AudioSource soleumAudioSource;
    [SerializeField] private AudioSource generalAudioSource;


    [SerializeField] private float happyVolume, soleumVolume;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //PlayBGM(AudioType.Happy, 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            //PlayBGM(AudioType.Soleum, 1);
        }
    }

    public void PlayBGM(AudioType audioType, AudioClip audioClip, float flipTime) {
        AudioSource sourceTurnOn = null, sourceTurnOff = null;
        float targetVolume = 0;
        switch (audioType) {
            case AudioType.Happy:
                sourceTurnOff = soleumAudioSource;
                sourceTurnOn = happyBGMSource;
                targetVolume = happyVolume;
                happyBGMSource.clip = audioClip;
                happyBGMSource.Play();
                break;
            case AudioType.Soleum:
                sourceTurnOff = happyBGMSource;
                sourceTurnOn = soleumAudioSource;
                targetVolume = soleumVolume;
                soleumAudioSource.clip = audioClip;
                soleumAudioSource.Play();
                break;
        }

        DOTween.To(() => sourceTurnOn.volume, v => sourceTurnOn.volume = v, targetVolume, flipTime);
        DOTween.To(() => sourceTurnOff.volume, v => sourceTurnOff.volume = v, 0, flipTime);
    }

    public void PlayAudioShot(AudioClip clip, float volumeScale) {
        generalAudioSource.PlayOneShot(clip, volumeScale);
    }

  
}

public struct EventA {
    public string Message;
    public int Number;

    public EventA(string msg, int num) {
        this.Message = msg;
        this.Number = num;
    }
}

public class TestClass: MonoBehaviour, ISingleton {
    public void OnSingletonInit() {
        
    }

    private void Awake() {
        TypeEventSystem.RegisterGlobalEvent<EventA>(Test).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    public void Test(EventA e) {

    }

    public static TestClass Singleton {
        get {
            return SingletonProperty<TestClass>.Singleton;
        }
    }
}