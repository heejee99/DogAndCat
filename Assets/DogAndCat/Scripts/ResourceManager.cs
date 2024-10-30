using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonManager<ResourceManager>
{
    //Dogs�� ����� ����Ʈ
    private List<Dogs> dogList = new List<Dogs>(); 

    //Cats�� ����� ����Ʈ
    private List<Cats> catList = new List<Cats>();

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
        Dogs dog = Instantiate(dogList[id - 1]);

        GameManager.Instance.dogs.Add(dog);

        return dog;
    }

    public Cats SpawnCat(int id)
    {
        Cats cat = Instantiate(catList[id - 1]);

        GameManager.Instance.cats.Add(cat);

        return cat;
    }
}
