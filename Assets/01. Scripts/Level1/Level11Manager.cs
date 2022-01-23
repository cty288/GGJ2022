using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework;
using MikroFramework.BindableProperty;
using MikroFramework.Event;
using MikroFramework.Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level11Manager :  MonoMikroSingleton<Level11Manager> {
    [SerializeField] private List<GameObject> enemyList;

    [SerializeField] private float spawnInterval = 4f;

   
    private float timer = 0;
    private float enemyNum = 0;
    public float EnemyNum => enemyNum;
    private bool canSpawn = false;
    private bool levelPassSend = false;
    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreePassed).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.RegisterGlobalEvent<OnLeftStart>(OnLeftStart).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnLeftStart(OnLeftStart obj) {
        canSpawn = true;
    }

    private void OnTreePassed(OnTreePassed e) {
        
        if (e.IsLastTree) {
            canSpawn = false;
        }
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) {
            timer = 0;
            if (canSpawn && enemyNum<=Level12Manager.Singleton.MaxTreeCount) {
                SpawnAEnemy();
                enemyNum++;
            }
        }

        if (Level12Manager.Singleton.CurrentTree == Level12Manager.Singleton.MaxTreeCount) {
            if (!GameObject.FindObjectOfType<L11Enemy>()) {
                if (!levelPassSend) {
                    levelPassSend = true;
                    Debug.Log("Level pass");
                    TypeEventSystem.SendGlobalEvent<OnLevelPass>(new OnLevelPass());
                }
            }
        }
    }

    [SerializeField] private Vector2 xRange = new Vector2(-150, 37);

    private void SpawnAEnemy() {
        Vector2 playerPos = L11Player.Singleton.transform.position;
        bool xRight = Random.Range(0, 2) == 0;
        float xPos = xRight ? playerPos.x + 16 + Random.Range(0, 10) : playerPos.x - 10 - Random.Range(0, 10);
        xPos = Mathf.Clamp(xPos, xRange.x, xRange.y);
        float yPos = Random.Range(-7f, 7f);
        int index = Random.Range(0, enemyList.Count);

        GameObject enemyToSpawn = Instantiate(enemyList[index], new Vector2(xPos, yPos),
            Quaternion.identity);
    }
}
