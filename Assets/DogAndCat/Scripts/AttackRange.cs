using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private List<Cats> detectedCatList = new List<Cats>();
    private Dogs dog;


    private float targetDistance = float.MaxValue; //������ �Ÿ�
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
                print("����̰� ����");
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
