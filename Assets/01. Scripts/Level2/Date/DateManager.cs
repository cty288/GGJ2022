using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Singletons;

public class DateManager : MonoMikroSingleton<DateManager>
{
    public GameObject hintPref;
    [SerializeField] private List<GameObject> hintSpawnPosList;
    [SerializeField] private List<GameObject> incorrectHintList;
    [SerializeField] private GameObject correctHint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public GameObject hint_W;
    public GameObject hint_A;
    public GameObject hint_S;
    public GameObject hint_D;


    #region Functional Field
    private void Awake()
    {
        SpawnHint();
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
        Destroy(target);
    }

    public void LaberKeyCode()
    {

        hint_W = incorrectHintList[0];
        hint_A = incorrectHintList[1];
        hint_S = incorrectHintList[2];
        hint_D = incorrectHintList[3];
    }

    public void SpawnHint()
    {
        if (correctHint != null) Destroy(correctHint);
        if (incorrectHintList.Count != 0)
        {
            foreach (GameObject element in incorrectHintList)
            {
                incorrectHintList.Remove(element);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject hint = Instantiate(hintPref, hintSpawnPosList[i].transform.position, Quaternion.identity);
            hint.name = "Hint " + i;
            incorrectHintList.Add(hint);
        }
        LaberKeyCode();
        correctHint = incorrectHintList[Random.Range(0, 4)];
        correctHint.name = "CorrectHint";
        incorrectHintList.Remove(correctHint);
        spriteRenderer = correctHint.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
    }

    public void CheckAnswer()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (correctHint == hint_W)
            {
                Debug.Log("Correct");
                DateManager.Singleton.SpawnHint();
            }
            else Debug.Log("Incorrect");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (correctHint == hint_A)
            {
                Debug.Log("Correct");
                DateManager.Singleton.SpawnHint();
            }
            else Debug.Log("Incorrect");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (correctHint == hint_S)
            {
                Debug.Log("Correct");
                DateManager.Singleton.SpawnHint();
            }
            else Debug.Log("Incorrect");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (correctHint == hint_D)
            {
                Debug.Log("Correct");
                DateManager.Singleton.SpawnHint();
            }
            else Debug.Log("Incorrect");
        }
    }

    #endregion

}
