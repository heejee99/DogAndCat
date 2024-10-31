using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        var tmp = new List<Dogs>();
        tmp = detectedDogList.ToList();
        if (tmp.Count > 0 && Time.time >= preDamageTime + cat.attackInterval)
        {
            for (int i = 0; i < tmp.Count; i++)
            {

                if (tmp[i] != null)
                {
                    tmp[i].TakeDamage(cat.damage);
                    if (tmp[i] != null)
                        print($"강아지 남은 체력{tmp[i].hp}");
                }
                else if (tmp[i].hp <= 0)
                {
                    detectedDogList.RemoveAt(i);
                }
                preDamageTime = Time.time;
            }
        }

        if (detectedDogList.Count == 0)
        {
            cat.isContact = false;
        }
    }

    private float preDamageTime;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Dogs dogs))
        {
            if (!detectedDogList.Contains(dogs))
            {
                detectedDogList.Add(dogs);

                cat.isContact = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            Dogs dogs = collision.GetComponent<Dogs>();
            if (detectedDogList.Contains(dogs))
            {
                detectedDogList.Remove(dogs);
                dogs.isContact = false;
            }
        }
    }
}
