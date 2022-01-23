using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
public class comicPlay : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject blackObject;

    public GameObject hint;
    private TMP_Text text;

    public Image image1;
    public Image image2;
    public Image image3;
    public Image black;

    public bool image1out = false;
    public bool image2out = false;
    public bool image3out = false;

    public bool image3turn = false;
    public bool gameout = false;

    void Start()
    {
        object1 = GameObject.Find("Image1");
        object2 = GameObject.Find("Image2");
        object3 = GameObject.Find("Image3");
        blackObject = GameObject.Find("Black");

        hint = GameObject.Find("Hint");

        Image image1 = object1.GetComponent<Image>();
        Image image2 = object2.GetComponent<Image>();
        Image image3 = object3.GetComponent<Image>();
        Image black = blackObject.GetComponent<Image>();

        text = hint.GetComponent<TMP_Text>();
        text.enabled = false;

        StartCoroutine(WaitForSecs());
    }

    IEnumerator WaitForSecs()
    {
        yield return new WaitForSeconds(1f);
        image1.DOFade(1, 1);
        image1out = true;
        text.enabled = true;
    }
    IEnumerator WaitForSecs2()
    {
        yield return new WaitForSeconds(1f);
        image3turn = true;
    }
    IEnumerator WaitForSecs3()
    {
        yield return new WaitForSeconds(1f);
        gameout = true;
    }

    IEnumerator WaitForSecs4()
    {
        yield return new WaitForSeconds(1f);
        text.enabled = false;
        black.DOFade(1, 1).OnComplete(() => {
            Timer.Singleton.AddDelayTask(1f, () => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });
        });
        
    }

    // Update is called once per frame
    void Update(){
        if(image1out == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                image2.DOFade(1, 1);
                image2out = true;
                StartCoroutine(WaitForSecs2());
            }
        }

        if ((image2out == true) &&(image3turn == true))
        {
            if (Input.GetMouseButtonDown(0))
            {
                image3.DOFade(1, 1);
                image3out = true;
                StartCoroutine(WaitForSecs3());
            }
        }

        if((image3out == true) && (gameout == true))
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(WaitForSecs4());
            }
        }

    }
}
