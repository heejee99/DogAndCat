using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttackRange : MonoBehaviour
{
    //���� �ȿ� �ִ� ����̵��� �޾��� ����Ʈ
    private List<Dogs> detectedDogList = new List<Dogs>();
    //�θ�� �ִ� ������Ʈ�� ����
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
                    cat.isContact = true; //isContact -> true�� �ٲ���
                    foreach (var dog in detectedDogList)
                    {
                        dog.TakeDamage(cat.damage);
                        print("�������� ����");
                        print($"���� ������ ü��{dog.hp}");
                    }
                    //if (dogs.IsDead())
                }
            }
        }
    }
}
