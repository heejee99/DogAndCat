using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //ũ��
    public float attackSpeed = 1f; //���ݼӵ�
    public float attackRange = 1f; //���ݹ���
    public float hp = 50; //ü��
    private float maxHp; //�ִ� ü��
    public float damage = 8; //���ݷ�
    public float cost = 200; //������

    public bool attackType; //���� Ÿ��

    public bool isContact = false; //���� �����°�?


    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation AnimalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.

    private void Start()
    {
        //StartCoroutine(OnAttackRange());
        maxHp = hp;
        GameManager.Instance.dogs.Add(this);
    }

    private void Update()
    {
        //���� �ȸ����� ����
        if (isContact)
        {
            Vector2 stop = Vector2.zero;
            Move(stop);
            AnimalAnimation.Jump();
        }
        //�� ������ ������
        else
        {
            Vector2 gLeft = Vector2.left;
            Move(gLeft);
            AnimalAnimation.Walk();
        }
        //Attack();
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void Attack()
    {

    }
    

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Cats>(out Cats cats))
    //    {
    //        GameManager.Instance.cats.Add(cats);
    //        foreach (var cat in GameManager.Instance.cats)
    //        {
    //            cat.TakeDamage(attack);
    //            print(cat.hp);
    //        }
    //    }
    //}

    ////�� �ݶ��̴��� ������
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //   if(collision.TryGetComponent<Cats>(out Cats cats))
    //    {
    //        isContact = true; 
    //    }
    //}

    //IEnumerator OnAttackRange()
    //{
    //    while (true)
    //    {
    //        Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //        foreach (Collider2D contactedColl in contactedColls)
    //        {
    //            if (contactedColl.CompareTag("Cat"))
    //            {
    //                print($"���� ������ �ִ� ����� �� : {contactedColls.Length}");
    //            }
    //        }
    //        yield return null;
    //    }
    //}

    //private void Attack()
    //{
    //    Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //    foreach (Collider2D contactedColl in contactedColls)
    //    {
    //        if (contactedColl.CompareTag("Cat"))
    //        {
    //            print($"���� ������ �ִ� ����� �� : {contactedColls.Length}");
    //        }
    //    }

    //}

}
