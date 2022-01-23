using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scewdive : MonoBehaviour
{

    public float duration = .2f;

    // Update is called once per frame
    void Update()
    {
        transform.DORotate(Vector3.zero, duration);
    }

}
