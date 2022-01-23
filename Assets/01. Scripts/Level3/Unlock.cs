using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MikroFramework.Event;
public struct Success
{

}


public class Unlock : MonoBehaviour
{

    public Transform clipIns;
    public Transform keyHole;
    public SpriteRenderer stickRenderer;
    public SpriteRenderer repairRenderer;
    public SpriteRenderer tutorialRenderer;
    public Sprite stickAva;
    public Sprite stcikBreak;
    public Text countText;
    public float rotError = 5f;
    public float rotSpeed = .4f;
    public float resumeDuration = .4f;
    public float moveFullDuration = 1f;
    public float repairEffectFadeTime = 2f;
    public int stickLastingCount = 3;

    float targetRotation;

    bool gameFinish = false;
    bool corretAngle = false;
    int stickLasting = 3;
    private bool canUse = false;
    void Start()
    {
        stickLasting = stickLastingCount;
        targetRotation = Random.Range(rotError - 90f, 90f - rotError);
        Debug.Log(targetRotation);
        TypeEventSystem.RegisterGlobalEvent<CorrectAction>(ResumeStick);
        TypeEventSystem.RegisterGlobalEvent<OnLeftStart>(OnLeftStart).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnLeftStart(OnLeftStart obj) {
        canUse = true;
    }

    void ResumeStick(CorrectAction action)
    {
        if (stickLasting == 0)
        {
            stickRenderer.sprite = stickAva;
            stickLasting = stickLastingCount;
            //如果有的话，播放修复发卡的音效
            PlayClip(11);
            repairRenderer.DOFade(1, 0);
            repairRenderer.DOFade(0, repairEffectFadeTime);
        }
    }

    void Update()
    {
        if (canUse) {
            countText.text = stickLasting.ToString();
            if (tutorialRenderer.color.a == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) tutorialRenderer.DOFade(0, repairEffectFadeTime);
            }
            if (!gameFinish)
            {
                float transferZ = clipIns.rotation.eulerAngles.z;
                if (transferZ > 270 - 11.4514) transferZ -= 360;
                if (stickLasting != 0)
                {
                    if (Input.GetKey(KeyCode.Q) && transferZ <= 90)
                    {
                        clipIns.Rotate(new Vector3(0, 0, rotSpeed));
                    }
                    if (Input.GetKey(KeyCode.E) && transferZ >= -90)
                    {
                        clipIns.Rotate(new Vector3(0, 0, -rotSpeed));
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        float solveE = SolveError();
                        //在这里播放转动的音效
                        if (Random.Range(0, 1f) > 0.5f) PlayClip(3); else PlayClip(4);
                        keyHole.DORotate(new Vector3(0, 0, solveE - 90), moveFullDuration);
                        Invoke("FinishRotate", moveFullDuration);
                    }
                }
                if (!Input.GetKey(KeyCode.Space)) ResumeLock();
            }
        }
       
    }

    void FinishRotate()
    {
        if (corretAngle) UnlockSuccess();
        else UnlockFail();
    }

    void ResumeLock()
    {
        keyHole.DORotate(Vector3.zero, resumeDuration);
        CancelInvoke("FinishRotate");
    }

    float SolveError()
    {
        float transferZ = clipIns.rotation.eulerAngles.z;
        if (transferZ > 270 - 11.4514) transferZ -= 360;
        float ret = Mathf.Abs(transferZ - targetRotation);
        corretAngle = false;
        if (ret < rotError)
        {
            corretAngle = true;
            return 0;
        }
        return ret/2;
    }

    void UnlockSuccess()
    {
        //在这里播放成功开锁音效
        PlayClip(8);
        GetComponent<Animation>().Play();
        gameFinish = true;
        Success success = new Success();

        Timer.Singleton.AddDelayTask(3f, () => {
            TypeEventSystem.SendGlobalEvent(new OnLevelPass());
        });
        
    }

    void UnlockFail()
    {
        ShakeCamera.Shake(true, 5, 35, 0.3f);
        //在这里要播放未成功开锁音效
        PlayClip(0);
        stickLasting--;
        if (stickLasting == 0)
        {
            Invoke("BreakStick", resumeDuration);
        }
    }

    void BreakStick()
    {
        //在这里播放棍子断掉的音效
        PlayClip(10);
        stickRenderer.sprite = stcikBreak;
    }

    void PlayClip(int index)
    {
        PlayAudio playAudio = new PlayAudio();
        playAudio.index = index;
        TypeEventSystem.SendGlobalEvent(playAudio);
    }
}
