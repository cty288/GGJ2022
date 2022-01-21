using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Singletons;
using UnityEngine;

public class AudioManager : MonoPersistentMikroSingleton<AudioManager> {
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource generalAudioSource;

    public void PlayBGM(AudioClip bgm, float volume) {
        bgmSource.Stop();
        bgmSource.clip = bgm;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void PlayAudioShot(AudioClip clip, float volumeScale) {
        generalAudioSource.PlayOneShot(clip, volumeScale);
    }
}
