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
    public List<GameObject> spawnedMoles;
    public List<GameObject> availableHoleList;
    public List<int> occupiedHoleList;
    
    [SerializeField] private int nextIndex;
    [SerializeField] private GameObject molePref;
    [SerializeField] private GameObject duckPref;
    [SerializeField] private bool readyToSpawnDucks = false;
    
    #region Functional Field

    private void Awake()
    {
        
    }

    private void Start()
    {
        ResetMoles();
        Timer.Singleton.AddDelayTask(5, () => {
            readyToSpawnDucks = true;
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

        // clear existing moles
        if (spawnedMoles.Count != 0) foreach (GameObject element in spawnedMoles) Destroy(element);
        spawnedMoles.Clear();
        
        //初始化可用草丛
        for (int i = 0; i < 7; i++) 
        {
            GameObject tempMole = moleSpawnPointsList[i];
            availableHoleList.Add(tempMole);
        }

        yield return new WaitForSeconds(2f);
        SpawnNewMole();

        yield return new WaitForSeconds(0.75f);
        SpawnNewMole();
        if (readyToSpawnDucks) SpawnNewDuck();

        yield return new WaitForSeconds(0.75f);
        SpawnNewMole();
        if (readyToSpawnDucks) SpawnNewDuck();

        availableHoleList.Clear();

        spawningMoles = false;
    }

    public void SpawnNewMole()
    {
        nextIndex = GetAvileableHoleIndex();
        GameObject newMole = Instantiate(molePref, availableHoleList[nextIndex].transform.position, Quaternion.identity);
        MoleClick moleClick = newMole.GetComponent<MoleClick>();
        moleClick.id = nextIndex;
        spawnedMoles.Add(newMole);
        occupiedHoleList.Add(nextIndex);
    }

    public int GetAvileableHoleIndex()
    {
        int i = Random.Range(0, 6);
        while (occupiedHoleList.Contains(i))
        {
            i = Random.Range(0, 6);
        }
        return i;
    }

    #endregion

    #region Ducks

    public void SpawnNewDuck()
    {
        nextIndex = GetAvileableHoleIndex();
        GameObject newDuck = Instantiate(duckPref, availableHoleList[nextIndex].transform.position, Quaternion.identity);
        DuckClick duckClick = newDuck.GetComponent<DuckClick>();
        duckClick.id = nextIndex;
        spawnedMoles.Add(newDuck);
        occupiedHoleList.Add(nextIndex);
    }


    #endregion
}


