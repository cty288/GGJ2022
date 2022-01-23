using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using UnityEngine;

public class VerySmallFirePeed : MonoBehaviour {

    public int TreeNum = -1;
    // Start is called before the first frame update
    void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreePassed).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnTreePassed(OnTreePassed e) {
        if (Level12Manager.Singleton.CurrentTree - 1 == TreeNum) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            DOTween.To(() => renderer.color, value => renderer.color = value,
                    new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0), 1f)
                .OnComplete(() => Destroy(this.gameObject));
        }
    }

}
