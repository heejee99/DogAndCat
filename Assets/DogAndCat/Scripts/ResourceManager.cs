using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonManager<ResourceManager>
{
    //Dogs�� ����� ����Ʈ
    private List<Dogs> dogList = new List<Dogs>(); 

    //Cats�� ����� ����Ʈ
    private List<Cats> catList = new List<Cats>();

    //��ġ�� �ʰ� ���� ����
    private int spawnIdx_Z = 0;

    public float maxSpawnIdx_Y = 3f;
    public float minSpawnIdx_Y = 0.3f;

    //�ڷ����� virtual�� �θ��� ���¿� ���ƾ� �Ѵ�.
    protected override void Awake()
    {
        base.Awake();
        for (int i = 1; i<=5; i++) 
        {
            //��üȭ ��Ų�� �ƴ����� Resources���� ������ �����ͼ� dogList�� ��Ƶд�
            Dogs dog = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Dog_{i}").GetComponent<Dogs>();
            dogList.Add(dog);
        }

        for (int i = 1; i <=3; i++)
        {
            Cats cat = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Cat_{i}").GetComponent<Cats>();
            catList.Add(cat);
        }

    }

    //��üȭ(GameManager�� ��� ��)�� �� �޼���
    public Dogs SpawnDog(int id)
    {
        spawnIdx_Z--;
        float spawnIdx_Y = Random.Range(minSpawnIdx_Y, maxSpawnIdx_Y);
        Vector3 spawnPos = new Vector3(GameManager.Instance.player.transform.position.x, GameManager.Instance.player.transform.position.y - spawnIdx_Y, GameManager.Instance.player.transform.position.z - spawnIdx_Z);
        Dogs dog = Instantiate(dogList[id - 1], spawnPos, Quaternion.identity);

        GameManager.Instance.dogs.Add(dog);

        return dog;
    }

    public Cats SpawnCat(int id)
    {
        spawnIdx_Z--;
        float spawnIdx_Y = Random.Range(minSpawnIdx_Y, maxSpawnIdx_Y);
        Vector3 spawnPos = new Vector3(GameManager.Instance.enemy.transform.position.x, GameManager.Instance.enemy.transform.position.y - spawnIdx_Y, GameManager.Instance.enemy.transform.position.z - spawnIdx_Z);
        Cats cat = Instantiate(catList[id - 1], spawnPos, Quaternion.identity);

        GameManager.Instance.cats.Add(cat);

        return cat;
    }
}
