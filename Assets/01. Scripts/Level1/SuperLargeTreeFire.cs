using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MikroFramework.Event;
using UnityEngine;

public class SuperLargeTreeFire : MonoBehaviour {
    [SerializeField] private Vector2 localXRange;
    [SerializeField] private Vector2 localYRange;

    [SerializeField] private GameObject[] firePrefabs;

    [SerializeField] private int maxSpawnCount = 5;

    private int spawnCount;
    private List<GameObject> spawnedTree;
    void Start() {
        spawnedTree = new List<GameObject>();
        spawnCount = Random.Range(2, maxSpawnCount+1);
        SpawnFires();
        TypeEventSystem.RegisterGlobalEvent<OnTreePassed>(OnTreePassed).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnTreePassed(OnTreePassed e) {
        if (e.IsLastTree) {
            foreach (GameObject tree in spawnedTree) {
                SpriteRenderer renderer = tree.GetComponentInChildren<SpriteRenderer>();
                DOTween.To(() => renderer.color, value => renderer.color = value,
                        new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0), 1f)
                    .OnComplete(() => Destroy(tree));

            }
        }
    }

    private void SpawnFires() {
        for (int i = 0; i < spawnCount; i++) {
            Vector2 pos = new Vector2(Random.Range(localXRange.x, localXRange.y),
                Random.Range(localYRange.x, localYRange.y));

            int index = Random.Range(0, firePrefabs.Length);
            GameObject fire = Instantiate(firePrefabs[index], new Vector2(), Quaternion.identity);
            fire.transform.SetParent(transform);
            fire.transform.localPosition = pos;
            spawnedTree.Add(fire);
        }
    }

    void Update()
    {
        
    }
}
