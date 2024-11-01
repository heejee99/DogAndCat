using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float gold = 0f;
    public int level = 1;
    public int exp = 0;
    public float hp;

    public int value = 6;

    private void Awake()
    {
        GameManager.Instance.player = this;
    }


    private void Start()
    {
    }

    private void Update()
    {
        Gold();
    }

    public void SpawnButton(int id)
    {
        ResourceManager.Instance.SpawnDog(id);
    }

    public float Gold()
    {
        gold += (1/value) * Time.deltaTime * 100;
        return gold;
    }
}
