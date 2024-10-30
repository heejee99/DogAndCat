using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackRange : MonoBehaviour
{
    //범위 안에 있는 고양이들을 받아줄 리스트
    private List<Cats> detectedCatList = null;
    //부모로 있는 오브젝트를 받음
    private Dogs dog;


    //private float targetDistance = float.MaxValue; //적과의 거리
    //private Cats targetCat = null;

    private void Awake()
    {
        dog = transform.GetComponentInParent<Dogs>();
    }
    private void Update()
    {
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(dog.transform.position, new Vector2(dog.attackRange, 2 * dog.attackRange), 0);
    
        //foreach(Collider2D collider in colliders)
        //{
        //    if (collider.CompareTag("Cat"))
        //    {
        //        Cats cat = collider.GetComponent<Cats>();

        //        if(cat != null && cat.GetComponent<Rigidbody2D>() != null)
        //        {
        //            dog.AnimalAnimation.Jump();
        //            cat.TakeDamage(dog.damage);
        //            print("고양이 공격 받음");
        //        }
        //    }

        //}
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Cats>(out Cats cats))
        {
            //중복 검사
            if (!detectedCatList.Contains(cats))
            {
                //처음 보는 얘면 리스트에 추가해줌
                detectedCatList.Add(cats);
                //근데 그 리스트가 0이 아닐때 까지
                while (detectedCatList.Count != 0)
                {
                    //정지
                    dog.isContact = true;
                    //GameManager.Instance.cats.Add(cats); //만난 고양이들을 리스트에 담아줌
                    //dog.IsContact(); //isContact -> true로 바꿔줌

                    //리스트에 있는 고양이들 전부
                    foreach (var cat in detectedCatList)
                    {
                        //데미지 입음
                        cat.TakeDamage(dog.damage);
                        print("고양이가 맞음");
                        print($"남은 고양이 체력{cat.hp}");
                    }
                    //근데 고양이 죽으면
                    if (cats.IsDead())
                    {
                        dog.isContact = false;
                        return;
                    }
                }

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
