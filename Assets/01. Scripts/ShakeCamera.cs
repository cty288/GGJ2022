using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public static class ShakeCamera{
    public static Camera CameraLeft
    {
        get
        {
            return GameObject.Find("CameraLeft").GetComponent<Camera>();        
        }
     }

    public static Camera CameraRight
    {
        get
        {
            return GameObject.Find("CameraRight").GetComponent<Camera>();
        }
    }


    public static void Shake(bool isLeft, float strenth =5, int vibrato=10, float time = 0.5f)
    {
        Camera target = isLeft ? CameraLeft : CameraRight;
        target.DOShakePosition(time, strenth,vibrato);
    }

}
