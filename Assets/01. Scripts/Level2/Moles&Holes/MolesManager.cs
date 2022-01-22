using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikroFramework.Singletons;
using MikroFramework.BindableProperty;

public class MolesManager : MonoMikroSingleton<MolesManager>
{
    public static int count;
    public GameObject holes;
    [SerializeField] private List<GameObject> holePos;
    [SerializeField] private int nextIndex;
    [SerializeField] private GameObject molePref;

    #region Functional Field

    private void Awake()
    {
        for(int i = 0; i < 9; i++)
        {
            string index = "" + i;
            holePos[i] = holes.transform.Find(index).gameObject;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnMole());
    }

    private void Update()
    {
        if (MolesManager.count == 3)
        {
            Debug.Log("OneSetDone");
            StartCoroutine(MolesManager.Singleton.SpawnMole());
            MolesManager.count = 0;
        }
    }

    #endregion

    #region Coroutine

    public IEnumerator SpawnMole()
    {
        yield return new WaitForSeconds(2f);
        nextIndex = Random.Range(0, 8);
        Instantiate(molePref, holePos[nextIndex].transform.position, Quaternion.identity);
        holePos.RemoveAt(nextIndex);

        yield return new WaitForSeconds(0.75f);
        nextIndex = Random.Range(0, 7);
        Instantiate(molePref, holePos[nextIndex].transform.position, Quaternion.identity);
        holePos.RemoveAt(nextIndex);

        yield return new WaitForSeconds(0.75f);
        nextIndex = Random.Range(0, 6);
        Instantiate(molePref, holePos[nextIndex].transform.position, Quaternion.identity);
        holePos.RemoveAt(nextIndex);

        holePos.Clear();

        foreach(GameObject element in holePos)
        {
            holePos.Remove(element);
        }

        for (int i = 0; i < 9; i++)
        {
            string index = "" + i;
            holePos.Add(holes.transform.Find(index).gameObject);
        }

        yield return new WaitForSeconds(2f);
    }

    #endregion
}
