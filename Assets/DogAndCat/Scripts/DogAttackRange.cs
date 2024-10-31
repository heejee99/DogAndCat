using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackRange : MonoBehaviour
{
    //범위 안에 있는 고양이들을 받아줄 리스트
    private List<Cats> detectedCatList = new List<Cats>();
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
        if (detectedCatList.Count > 0 && Time.time >= preDamageTime + dog.damageInterval)
        {
            foreach (var cat in detectedCatList)
            {
                if (cat != null)
                {
                    cat.TakeDamage(dog.damage);
                    print($"고양이 남은 체력 : {cat.hp}");
                }
            }
            preDamageTime = Time.time;
        }
    }

    private float preDamageTime; //이전에 데미지를 준 시간(Time.time)

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Cats>(out Cats cats))
        {
            //중복 검사
            if (!detectedCatList.Contains(cats))
            {
                //처음 보는 얘면 리스트에 추가해줌
                detectedCatList.Add(cats);
                //근데 그 리스트가 0이 아닐때 까지
                //정지
                dog.isContact = true;

                ////근데 고양이 죽으면
                //if (cats.IsDead())
                //{
                //    dog.isContact = false;
                //    return;
                //}
            }
        }

        //if (collision.CompareTag("Cat"))
        //{
        //    Cats cats = collision.GetComponent<Cats>();
        //    detectedCatList.Add(cats);
        //    foreach(Cats cat in detectedCatList)
        //    {
        //        cat.TakeDamage(dog.damage);
        //    }
        //}
    }

    //콜라이더 범위 벗어나면 detectedCatList에서 벗어나서 데미지 안입음
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            Cats cats = collision.GetComponent<Cats>();
            if (detectedCatList.Contains(cats))
            {
                detectedCatList.Remove(cats);
                dog.isContact = false;
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
