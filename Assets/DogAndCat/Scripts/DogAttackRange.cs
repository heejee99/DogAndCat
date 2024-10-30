using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAttackRange : MonoBehaviour
{
    //���� �ȿ� �ִ� ����̵��� �޾��� ����Ʈ
    private List<Cats> detectedCatList = null;
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
        //            print("����� ���� ����");
        //        }
        //    }

        //}
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Cats>(out Cats cats))
        {
            //�ߺ� �˻�
            if (!detectedCatList.Contains(cats))
            {
                //ó�� ���� ��� ����Ʈ�� �߰�����
                detectedCatList.Add(cats);
                //�ٵ� �� ����Ʈ�� 0�� �ƴҶ� ����
                while (detectedCatList.Count != 0)
                {
                    //����
                    dog.isContact = true;
                    //GameManager.Instance.cats.Add(cats); //���� ����̵��� ����Ʈ�� �����
                    //dog.IsContact(); //isContact -> true�� �ٲ���

                    //����Ʈ�� �ִ� ����̵� ����
                    foreach (var cat in detectedCatList)
                    {
                        //������ ����
                        cat.TakeDamage(dog.damage);
                        print("����̰� ����");
                        print($"���� ����� ü��{cat.hp}");
                    }
                    //�ٵ� ����� ������
                    if (cats.IsDead())
                    {
                        dog.isContact = false;
                        return;
                    }
                }

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
