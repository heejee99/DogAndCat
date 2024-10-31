using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackRange : MonoBehaviour
{
    //���� �ȿ� �ִ� ����̵��� �޾��� ����Ʈ
    private List<Cats> detectedCatList = new List<Cats>();
    //�θ�� �ִ� ������Ʈ�� ����
    private Dogs dog;


    //private float targetDistance = float.MaxValue; //������ �Ÿ�
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
                    print($"����� ���� ü�� : {cat.hp}");
                }
            }
            preDamageTime = Time.time;
        }
    }

    private float preDamageTime; //������ �������� �� �ð�(Time.time)

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Cats>(out Cats cats))
        {
            //�ߺ� �˻�
            if (!detectedCatList.Contains(cats))
            {
                //ó�� ���� ��� ����Ʈ�� �߰�����
                detectedCatList.Add(cats);
                //�ٵ� �� ����Ʈ�� 0�� �ƴҶ� ����
                //����
                dog.isContact = true;

                ////�ٵ� ����� ������
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

    //�ݶ��̴� ���� ����� detectedCatList���� ����� ������ ������
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

    //�� �ݶ��̴��� ������, �ٵ� �Ʒ����� ���������� �ȵ� ���� ���� ���� Ÿ������ �Ѹ����� ����
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
