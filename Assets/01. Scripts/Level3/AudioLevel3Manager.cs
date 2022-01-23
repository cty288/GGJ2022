using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Event;

public struct PlayAudio
{
    public int index;
}

public class AudioLevel3Manager : MonoBehaviour
{

    public AudioClip[] audioClips;

    private void Start()
    {
        TypeEventSystem.RegisterGlobalEvent<PlayAudio>(PlayAudio);
    }
    //private audiomanager() { }


    void PlayAudio(PlayAudio playAudio)
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[playAudio.index], 1);
    }
}
