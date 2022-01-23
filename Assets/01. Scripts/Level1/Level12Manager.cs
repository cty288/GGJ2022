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
    public GameObject treeShootAnimationPrefab;
}

public struct OnTreeSpawned {
    public PeeTree PeeTree;
    public Vector2 SpawnPosition;
}

public struct OnTreePassed {
    public bool IsLastTree;
    public PeeTree PeeTree;
}

public class Level12Manager : MonoMikroSingleton<Level12Manager> {
    [SerializeField] public List<PeeTree> PeeTreeConfig;
  
    [SerializeField] private float treeSpawnY;
    [SerializeField] private float treeInitialSpawnX;
    [SerializeField] private float treeGap;
    [SerializeField] private int maxTreeCount;
    public int MaxTreeCount => maxTreeCount;

    [SerializeField]
    private int currentTree = 0;
    public int CurrentTree => currentTree;

    public PeeTree CurrentPeeTree;

    private void Start() {
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreeSwitched).UnRegisterWhenGameObjectDestroyed(gameObject);
        SwitchTree(Random.Range(0, PeeTreeConfig.Count-1));
    }

    private void OnTreeSwitched(OnTreePassed e) {
        Timer.Singleton.AddDelayTask(2f, () => {
            Debug.Log("Switch tree");
            if (currentTree < maxTreeCount)
            {
                if (currentTree == maxTreeCount - 1)
                {
                    SwitchTree(PeeTreeConfig.Count - 1);
                }
                else
                {
                    int switchIndex = Random.Range(0, PeeTreeConfig.Count - 1);
                    SwitchTree(switchIndex);

                }

            }
        });
        
        
    }

    private void SwitchTree(int treeIndex) {
        Vector2 spawnPosition = new Vector2(treeInitialSpawnX + currentTree * treeGap, treeSpawnY);
        PeeTree peeTree = PeeTreeConfig[treeIndex];
        GameObject treeSpawned = Instantiate(peeTree.TreePrefab, spawnPosition, Quaternion.identity);
         if (treeSpawned.TryGetComponent<VerySmallFirePeed>(out VerySmallFirePeed tree)) {
             tree.TreeNum = currentTree;
         }
        TypeEventSystem.SendGlobalEvent<OnTreeSpawned>(new OnTreeSpawned() {PeeTree = peeTree, SpawnPosition = spawnPosition});
        currentTree++;
        CurrentPeeTree = peeTree;
    }
}
