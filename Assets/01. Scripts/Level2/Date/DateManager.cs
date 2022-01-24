using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Singletons;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using MikroFramework.Event;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DateManager : MonoMikroSingleton<DateManager>
{
    public GameObject hint_WPref;
    public GameObject hint_APref;
    public GameObject hint_SPref;
    public GameObject hint_DPref;

    public GameObject linkedBtn;

    [SerializeField] private List<GameObject> hintSpawnPosList;
    [SerializeField] private List<GameObject> incorrectHintList;
    [SerializeField] private List<GameObject> btnList;
    public List<GameObject> btnOrginPosLis;
    [SerializeField] private GameObject correctHint;
    [SerializeField] private GameObject talk_Right;
    [SerializeField] private GameObject talk_Left;

    [SerializeField] private GameObject dogGirl;
    [SerializeField] private GameObject dogGuy;

    private Animator rightAnimator;
    private Animator leftAnimator;

    private Animator dogGirlAnimator;
    private Animator dogGuyAnimator;


    public GameObject hint_Up;
    public GameObject hint_Left;
    public GameObject hint_Down;
    public GameObject hint_Right;

    public int correctHintIndex;
    public int matchIndex = -2;
    public int leftPlayIndex;

    public bool isShaking = false;

    public AudioClip correctChoice1;
    public AudioClip correctChoice2;
    public AudioClip correctChoice3;
    public AudioClip wrongChoice;

    public int streak = 0;

    
    #region Functional Field
    private void Awake()
    {
        rightAnimator = talk_Right.GetComponent<Animator>();
        leftAnimator = talk_Left.GetComponent<Animator>();
        dogGirlAnimator = dogGirl.GetComponent<Animator>();
        dogGuyAnimator = dogGuy.GetComponent<Animator>();
        Timer.Singleton.AddDelayTask(3f, () =>
        {
            
                ResetHints();
            
           
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
            CheckAnswer();
        
       
    }

  

    #region Talk

    

    #endregion


    #endregion

    #region

    public void DestroyHint()
    {
       
            GameObject target = incorrectHintList[Random.Range(0,incorrectHintList.Count)];
            incorrectHintList.Remove(target);
            SpriteRenderer render = target.GetComponent<SpriteRenderer>();
            render.DOFade(0, 0.9f).OnComplete(() => {
                Destroy(target);
            });

            ButtonFadeIn script = target.GetComponent<ButtonFadeIn>();
            linkedBtn = script.linkedButton;
            render = linkedBtn.GetComponent<SpriteRenderer>();
            render.DOFade(0, 0.9f);
        
      
    }

    public void LaberKeyCode()
    {
        hint_Up = incorrectHintList[0];
        hint_Left = incorrectHintList[1];
        hint_Down = incorrectHintList[2];
        hint_Right = incorrectHintList[3];
    }
    
    public void CheckAnswer()
    {

        leftPlayIndex = 0;

        if (Input.GetKeyDown(KeyCode.W)) {
          
            while (btnOrginPosLis[leftPlayIndex] != hint_Up.GetComponent<ButtonFadeIn>().linkedButton)
            {
                leftPlayIndex++;
            }
            leftAnimator.SetInteger("State", leftPlayIndex);
            if (correctHint == hint_Up) CorrectChoice();
            else WrongChoice();
            
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
           
            while (btnOrginPosLis[leftPlayIndex] != hint_Left.GetComponent<ButtonFadeIn>().linkedButton)
            {
                leftPlayIndex++;
            }
            leftAnimator.SetInteger("State", leftPlayIndex);
            if (correctHint == hint_Left) CorrectChoice();
            else WrongChoice();
            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
           
            while (btnOrginPosLis[leftPlayIndex] != hint_Down.GetComponent<ButtonFadeIn>().linkedButton)
            {
                leftPlayIndex++;
            }
            leftAnimator.SetInteger("State", leftPlayIndex);
            if (correctHint == hint_Down) CorrectChoice();
            else WrongChoice();
           
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            
            while (btnOrginPosLis[leftPlayIndex] != hint_Right.GetComponent<ButtonFadeIn>().linkedButton)
            {
                leftPlayIndex++;
            }
            leftAnimator.SetInteger("State", leftPlayIndex);
            if (correctHint == hint_Right) CorrectChoice();
            else WrongChoice();
            
        }

       
    }

 
    public void CorrectChoice()
    {
        streak++;
        if (guessCorrectCounter == -1) {
            guessCorrectCounter = 0;
        }
        guessCorrectCounter++;
        if (streak == 0)
        {
            AudioManager.Singleton.PlayAudioShot(correctChoice1, 1);
        }
        else if (streak == 1)
        {
            AudioManager.Singleton.PlayAudioShot(correctChoice2, 1);
        }
        else 
        {
            AudioManager.Singleton.PlayAudioShot(correctChoice3, 1);
        }

        if (guessCorrectCounter == 5) {
            TypeEventSystem.SendGlobalEvent<OnLevelPass>(new OnLevelPass());
        }
        dogGuyAnimator.SetInteger("State", 0);
        dogGirlAnimator.SetInteger("State", 0);
        Timer.Singleton.AddDelayTask(3f, () =>
        {
            Debug.Log("Correct");
            DateManager.Singleton.ResetHints(); //Reset Hints
            if (!MolesManager.Singleton.spawningMoles) MolesManager.Singleton.ResetMoles(); //Reset Moles
            MolesManager.Singleton.occupiedHoleList.Clear(); //remove occupied list
            leftAnimator.SetInteger("State", -1);
        });
        
    }

    private int guessCorrectCounter = -1;
    public void WrongChoice()
    {
        TypeEventSystem.SendGlobalEvent<OnPlayerFail>();

        AudioManager.Singleton.PlayAudioShot(wrongChoice, 1);
        streak = 0;
        ShakeCamera.Shake(true, 3, 10);

        dogGuyAnimator.SetInteger("State", 1);
        dogGirlAnimator.SetInteger("State", 1);
        Timer.Singleton.AddDelayTask(3f, () =>
        {
            if (guessCorrectCounter != -1) {
                Debug.Log("You Loser");


                DateManager.Singleton.ResetHints();
                if (!MolesManager.Singleton.spawningMoles) MolesManager.Singleton.ResetMoles();
                MolesManager.Singleton.occupiedHoleList.Clear(); //remove occupied list
                if (leftAnimator)
                {
                    leftAnimator.SetInteger("State", -1);
                }
            }
           
           
        });
    }

    public void ResetHints()
    {
        dogGuyAnimator.SetInteger("State", 0);
        dogGirlAnimator.SetInteger("State", 0);
        ClearPrevHints();
        SpawnNewHints();

        if (hintSpawnPosList.Count != 0)
        {
            foreach (GameObject element in hintSpawnPosList)
            {
                ButtonFadeIn script = element.GetComponent<ButtonFadeIn>();
                linkedBtn = script.linkedButton;
                if (linkedBtn != null)
                {
                    SpriteRenderer render = linkedBtn.GetComponent<SpriteRenderer>();
                    render.DOFade(1, 0.9f);
                }
            }
        }
    }

    public void ClearPrevHints()
    {
        if (correctHint != null) Destroy(correctHint);
        if (incorrectHintList.Count != 0)
        {
            foreach (GameObject element in incorrectHintList)
            {
                Destroy(element);
            }
            incorrectHintList.Clear();
        }
    }

    public void SpawnNewHints()
    {
        foreach(GameObject element in btnOrginPosLis) {
            element.GetComponent<SpriteRenderer>().DOFade(1, 1f);
        }

        for (int i = 0; i < hintSpawnPosList.Count; i++)
        {
            GameObject temp = hintSpawnPosList[i];
            int randomIndex = Random.Range(i, hintSpawnPosList.Count);
            hintSpawnPosList[i] = hintSpawnPosList[randomIndex];
            hintSpawnPosList[randomIndex] = temp;
        }

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                GameObject hint = Instantiate(hint_WPref, hintSpawnPosList[i].transform.position, Quaternion.identity);
                hint.name = "Hint_W";
                incorrectHintList.Add(hint);

                ButtonFadeIn script = hint.GetComponent<ButtonFadeIn>();
                script.linkedButton = hintSpawnPosList[i];
            }

            else if (i == 1)
            {
                GameObject hint = Instantiate(hint_APref, hintSpawnPosList[i].transform.position, Quaternion.identity);
                hint.name = "Hint_A";
                incorrectHintList.Add(hint);

                ButtonFadeIn script = hint.GetComponent<ButtonFadeIn>();
                script.linkedButton = hintSpawnPosList[i];
            }

            else if (i == 2)
            {
                GameObject hint = Instantiate(hint_SPref, hintSpawnPosList[i].transform.position, Quaternion.identity);
                hint.name = "Hint_S";
                incorrectHintList.Add(hint);

                ButtonFadeIn script = hint.GetComponent<ButtonFadeIn>();
                script.linkedButton = hintSpawnPosList[i];
            }

            else if (i == 3)
            {
                GameObject hint = Instantiate(hint_DPref, hintSpawnPosList[i].transform.position, Quaternion.identity);
                hint.name = "Hint_D";
                incorrectHintList.Add(hint);

                ButtonFadeIn script = hint.GetComponent<ButtonFadeIn>();
                script.linkedButton = hintSpawnPosList[i];
            }


        }
        LaberKeyCode();
        correctHintIndex = Random.Range(0, 4);
        correctHint = incorrectHintList[correctHintIndex];
        correctHint.name = "CorrectHint";
        incorrectHintList.Remove(correctHint);

        matchIndex = 0;
        while (btnOrginPosLis[matchIndex] != correctHint.GetComponent<ButtonFadeIn>().linkedButton)
        {
            matchIndex++;
        }

        rightAnimator.SetInteger("State", matchIndex);

        
    }

    #endregion

}
