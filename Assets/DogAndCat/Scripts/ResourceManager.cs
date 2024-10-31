using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonManager<ResourceManager>
{
    //Dogs를 담아줄 리스트
    private List<Dogs> dogList = new List<Dogs>(); 

    //Cats를 담아줄 리스트
    private List<Cats> catList = new List<Cats>();

    //겹치지 않게 해줄 변수
    private int spawnIdx_Z = 0;

    public float maxSpawnIdx_Y = 3f;
    public float minSpawnIdx_Y = 0.3f;

    //자료형은 virtual한 부모의 형태와 같아야 한다.
    protected override void Awake()
    {
        base.Awake();
        for (int i = 1; i<=5; i++) 
        {
            //실체화 시킨건 아니지만 Resources에서 프리팹 가져와서 dogList에 담아둔다
            Dogs dog = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Dog_{i}").GetComponent<Dogs>();
            dogList.Add(dog);
        }

        for (int i = 1; i <=3; i++)
        {
            Cats cat = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Cat_{i}").GetComponent<Cats>();
            catList.Add(cat);
        }

    }

    //실체화(GameManager에 담는 것)를 할 메서드
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
