using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Singletons;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private GameObject correctHint;
     
    
    public GameObject hint_Up;
    public GameObject hint_Left;
    public GameObject hint_Down;
    public GameObject hint_Right;


    #region Functional Field
    private void Awake()
    {
        ResetHints();
    }

    private void Update()
    {
        CheckAnswer();
    }

    #endregion 

    #region

    public void DestroyHint()
    {
        GameObject target = incorrectHintList[Random.Range(0, incorrectHintList.Count)];
        incorrectHintList.Remove(target);
        SpriteRenderer render = target.GetComponent<SpriteRenderer>();
        render.DOFade(0, 0.9f).OnComplete(() => Destroy(target));

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (correctHint == hint_Up) CorrectChoice();
            else WrongChoice();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (correctHint == hint_Left) CorrectChoice();
            else WrongChoice();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (correctHint == hint_Down) CorrectChoice();
            else WrongChoice();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (correctHint == hint_Right) CorrectChoice();
            else WrongChoice();
        }
    }
 
    public void CorrectChoice()
    {
        Debug.Log("Correct");
        DateManager.Singleton.ResetHints(); //Reset Hints
        if (!MolesManager.Singleton.spawningMoles) MolesManager.Singleton.ResetMoles(); //Reset Moles
        MolesManager.Singleton.occupiedHoleList.Clear(); //remove occupied list
    } 

    public void WrongChoice()
    {
        Debug.Log("You Loser");
        DateManager.Singleton.ResetHints();
        if (!MolesManager.Singleton.spawningMoles) MolesManager.Singleton.ResetMoles();
        MolesManager.Singleton.occupiedHoleList.Clear(); //remove occupied list
    }

    public void ResetHints()
    {
        ClearPrevHints();
        SpawnNewHints();
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
        correctHint = incorrectHintList[Random.Range(0, 4)];
        correctHint.name = "CorrectHint";
        incorrectHintList.Remove(correctHint);
    }

    #endregion

}
