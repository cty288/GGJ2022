using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Singletons;
using MikroFramework.BindableProperty;

public class MolesManager : MonoMikroSingleton<MolesManager>
{
    public bool spawningMoles = false;

    public static int count;
    public List<GameObject> moleSpawnPointsList;
    public List<GameObject> moleTempList;
    public List<GameObject> spawnedMoles;

    private int nextIndex;
    [SerializeField] private GameObject molePref;
    [SerializeField] private GameObject duckPref;
    
    #region Functional Field

    private void Awake()
    {
        
    }

    private void Start()
    {
        ResetMoles();
        Timer.Singleton.AddDelayTask(5, () => {
            DuckSpawn(2);
        });
    }

    private void Update()
    {
        
    }

    #endregion

    #region Moles
    
    public void ResetMoles()
    {
        StartCoroutine(CoroutineResetMoles());
    }

    public IEnumerator CoroutineResetMoles()
    {
        spawningMoles = true;
        if (spawnedMoles.Count != 0) foreach (GameObject element in spawnedMoles) Destroy(element);
        spawnedMoles.Clear();

        moleTempList.Clear();

        for (int i = 0; i < 9; i++) 
        {
            GameObject tempMole = moleSpawnPointsList[i];
            moleTempList.Add(tempMole);
        }

        yield return new WaitForSeconds(2f);
        nextIndex = Random.Range(0, 8);
        spawnedMoles.Add(Instantiate(molePref, moleTempList[nextIndex].transform.position, Quaternion.identity));
        moleTempList.RemoveAt(nextIndex);

        yield return new WaitForSeconds(0.75f);
        nextIndex = Random.Range(0, 7);
        spawnedMoles.Add(Instantiate(molePref, moleTempList[nextIndex].transform.position, Quaternion.identity));
        moleTempList.RemoveAt(nextIndex);

        yield return new WaitForSeconds(0.75f);
        nextIndex = Random.Range(0, 6);
        spawnedMoles.Add(Instantiate(molePref, moleTempList[nextIndex].transform.position, Quaternion.identity));
        moleTempList.RemoveAt(nextIndex);

        spawningMoles = false;
    }

    #endregion

    #region Ducks

    public void DuckSpawn(float spawnRate)
    {
        while (!spawningMoles && moleTempList.Count != 0) {
            Instantiate(duckPref, moleTempList[Random.Range(0, moleTempList.Count)].transform.position, Quaternion.identity);
            Timer.Singleton.AddDelayTask(spawnRate, () =>
            {
                Instantiate(duckPref, moleTempList[Random.Range(0, moleTempList.Count)].transform.position, Quaternion.identity);
            });
        }
    }


    #endregion
}


