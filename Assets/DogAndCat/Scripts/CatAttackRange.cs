using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttackRange : MonoBehaviour
{
    //범위 안에 있는 고양이들을 받아줄 리스트
    private List<Dogs> detectedDogList = new List<Dogs>();
    //부모로 있는 오브젝트를 받음
    private Cats cat;


   

    private void Awake()
    {
        cat = transform.GetComponentInParent<Cats>();
    }
    private void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Dogs>(out Dogs dogs))
        {
            //print($"Cat : {detectedDogList.Count}");

            if (!detectedDogList.Contains(dogs))
            {
                detectedDogList.Add(dogs);
                //GameManager.Instance.dogs.Add(dogs);
                while (detectedDogList.Count != 0)
                {
                    cat.isContact = true; //isContact -> true로 바꿔줌
                    foreach (var dog in detectedDogList)
                    {
                        dog.TakeDamage(cat.damage);
                        print("강아지가 맞음");
                        print($"남은 강아지 체력{dog.hp}");
                    }
                    //if (dogs.IsDead())
                }
            }
        }
    }
}
