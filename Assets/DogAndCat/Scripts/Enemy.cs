using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 1000f;
    private float maxHp;

    public float delayTimeForSpawnCat_1 = 7f;
    public float delayTimeForSpawnCat_2 = 10f;
    public float delayTimeForSpawnCat_3 = 30f;

    public bool isDead = false;

    private void Awake()
    {
        GameManager.Instance.enemy = this;
    }
    private void Start()
    {   maxHp = hp; 
        StartCoroutine("SpawnCat_1", 1);
        StartCoroutine("SpawnCat_2", 2);
        StartCoroutine("SpawnCat_3", 3);
    }

    public IEnumerator SpawnCat_1(int id)
    {
        while (true)
        {
            ResourceManager.Instance.SpawnCat(id);
            yield return new WaitForSeconds(delayTimeForSpawnCat_1);
        }
    }
    public IEnumerator SpawnCat_2(int id)
    {
        yield return new WaitForSeconds(60f);
        while (true)
        {
            ResourceManager.Instance.SpawnCat(id);
            yield return new WaitForSeconds(delayTimeForSpawnCat_2);
        }
    }
    public IEnumerator SpawnCat_3(int id)
    {
        yield return new WaitForSeconds(120f);
        while (true)
        {
            ResourceManager.Instance.SpawnCat(id);
            yield return new WaitForSeconds(delayTimeForSpawnCat_3);
        }
    }

    public void TakeDamage(float damage)
    {
        if (hp < 0)
        {
            return;
        }

        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
        print("타워가 데미지를 입음");
    }

    public void OnDead()
    {
        if (isDead)
        {
            Destroy(gameObject);
        }
    }
}
