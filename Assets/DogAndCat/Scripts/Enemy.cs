using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;

    public int delayTimeForSpawnCat_1 = 7;
    public int delayTimeForSpawnCat_2 = 10;
    public int delayTImeForSpawnCat_3 = 30;

    private void Start()
    {
        StartCoroutine("SpawnCat_1", 1);
    }

    public IEnumerator SpawnCat_1(int id)
    {
        ResourceManager.Instance.SpawnCat(id);
        while (true)
        {
            ResourceManager.Instance.SpawnCat(id);
            yield return new WaitForSeconds(delayTimeForSpawnCat_1);
        }
    }
}
