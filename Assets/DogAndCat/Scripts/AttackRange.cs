using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private List<Cats> detectedCatList = new List<Cats>();
    private Dogs dog;


    private float targetDistance = float.MaxValue; //적과의 거리
    private Cats targetCat = null;

    private void Awake()
    {
        dog = transform.GetComponentInParent<Dogs>();
    }
    private void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Cats>(out Cats cats))
        {
            dog.IsContact();
            GameManager.Instance.cats.Add(cats);
            foreach (var cat in GameManager.Instance.cats)
            {
                cat.TakeDamage(dog.Attack);
                print("고양이가 맞음");
            }
        }
    }

    //적 콜라이더를 감지함, 근데 아래꺼는 범위공격이 안됨 공격 범위 내에 타겟팅이 한마리만 가능
    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Cats>(out Cats cats))
    //    {
    //        detectedCatList.Add(cats);
    //        foreach (var cat in detectedCatList)
    //        {
    //            float distance = Vector3.Distance(cat.transform.position, transform.position);

    //            if (distance < targetDistance)
    //            {
    //                targetDistance = distance;
    //                targetCat = cat;
    //            }
    //        }
    //    }
    //}
}
