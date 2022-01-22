using System;
using System.Collections;
using System.Collections.Generic;
using MikroFramework.Event;
using MikroFramework.Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PeeTree {
    public GameObject TreePrefab;
    public float PeeChargePosition;
}

public struct OnTreeSpawned {
    public PeeTree PeeTree;
    public Vector2 SpawnPosition;
}

public struct OnTreePassed {
    
}

public class Level12Manager : MonoMikroSingleton<Level12Manager> {
    [SerializeField] private List<PeeTree> PeeTreeConfig;
    [SerializeField] private float treeSpawnY;
    [SerializeField] private float treeInitialSpawnX;
    [SerializeField] private float treeGap;
    [SerializeField] private float maxTreeCount;

    private int currentTree = 0;

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreeSwitched).UnRegisterWhenGameObjectDestroyed(gameObject);
        SwitchTree();
    }

    private void OnTreeSwitched(OnTreePassed e) {
        if (currentTree < maxTreeCount) {
            SwitchTree();
        }
        
    }

    private void SwitchTree() {
        Vector2 spawnPosition = new Vector2(treeInitialSpawnX + currentTree * treeGap, treeSpawnY);

        PeeTree peeTree = PeeTreeConfig[Random.Range(0, PeeTreeConfig.Count)];
        GameObject treeSpawned = Instantiate(peeTree.TreePrefab, spawnPosition, Quaternion.identity);
        TypeEventSystem.SendGlobalEvent<OnTreeSpawned>(new OnTreeSpawned() {PeeTree = peeTree, SpawnPosition = spawnPosition});
        currentTree++;
    }
}
