using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonManager<ResourceManager>
{
    //Dogs를 담아줄 리스트
    private List<Dog> dogList = new List<Dog>(); 

    //Cats를 담아줄 리스트
    private List<Cat> catList = new List<Cat>();

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
            Dog dog = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Dog_{i}").GetComponent<Dog>();
            dogList.Add(dog);
        }

        for (int i = 1; i <=3; i++)
        {
            Cat cat = Resources.Load<GameObject>($"2D Cute Domestic Animal Pack V.2/Prefabs/Cat_{i}").GetComponent<Cat>();
            catList.Add(cat);
        }

    }

    //실체화(GameManager에 담는 것)를 할 메서드
    public Dog SpawnDog(int id)
    {
        spawnIdx_Z--;
        float spawnIdx_Y = Random.Range(minSpawnIdx_Y, maxSpawnIdx_Y);
        Vector3 spawnPos = new Vector3(GameManager.Instance.player.transform.position.x, GameManager.Instance.player.transform.position.y - spawnIdx_Y, GameManager.Instance.player.transform.position.z - spawnIdx_Z);
        Dog dog = Instantiate(dogList[id - 1], spawnPos, Quaternion.identity);

        GameManager.Instance.dog.Add(dog);

        return dog;
    }

    public Cat SpawnCat(int id)
    {
        spawnIdx_Z--;
        float spawnIdx_Y = Random.Range(minSpawnIdx_Y, maxSpawnIdx_Y);
        Vector3 spawnPos = new Vector3(GameManager.Instance.enemy.transform.position.x, GameManager.Instance.enemy.transform.position.y - spawnIdx_Y, GameManager.Instance.enemy.transform.position.z - spawnIdx_Z);
        Cat cat = Instantiate(catList[id - 1], spawnPos, Quaternion.identity);

        GameManager.Instance.cat.Add(cat);

        return cat;
    }
}
