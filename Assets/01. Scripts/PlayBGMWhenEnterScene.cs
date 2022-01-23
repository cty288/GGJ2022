using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGMWhenEnterScene : MonoBehaviour {
    [SerializeField] private AudioClip audioClipToPlay;
    [SerializeField] private AudioType audioType;

    private void Awake() {
        AudioManager.Singleton.PlayBGM(audioType, audioClipToPlay, 1.5f);
    }
}
