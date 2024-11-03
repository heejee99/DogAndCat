using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Collections.LowLevel.Unsafe;
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

    private void Start()
    {
        
    }
    private void Update()
    {
        var tmp = new List<Cats>();
        tmp = detectedCatList.ToList();
        if (tmp.Count > 0 && Time.time >= preDamageTime + dog.attackInterval)
        {
            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i] != null)
                {
                    tmp[i].TakeDamage(dog.damage);
                    if (tmp[i] != null)
                    print($"����� ���� ü�� : {tmp[i].hp}");
                }
                else if (tmp[i].hp <= 0)
                {
                    detectedCatList.RemoveAt(i);
                }
                preDamageTime = Time.time;

            }
        }
        if (detectedCatList.Count == 0)
        {
            dog.isContact = false;
        }
    }

    private float preDamageTime; //������ �������� �� �ð�(Time.time)

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Cats cats) )
        {
            //�ߺ� �˻�
            if (!detectedCatList.Contains(cats))
            {
                //ó�� ���� ��� ����Ʈ�� �߰�����
                detectedCatList.Add(cats);
                //�ٵ� �� ����Ʈ�� 0�� �ƴҶ� ����
                //����
                dog.isContact = true;
            }
        }

        //if (collision.CompareTag("Enemy"))
        //{
        //    StartCoroutine("AttackEnemyTower", dog.attackInterval);
        //    dog.isContact = true;
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

    private IEnumerator AttackEnemyTower(float attackInterval)
    {
        dog.isContact = true;
        while (true)
        {
            GameManager.Instance.enemy.TakeDamage(dog.damage);
            yield return new WaitForSeconds(attackInterval);
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
