using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //ũ��
    public float hp = 50; //ü��
    private float maxHp; //�ִ� ü��
    public float attack = 8; //���ݷ�
    public float Attack { get { return attack; } }

    public bool isContact = false; //���� �����°�?

    public float attackRange = 10f;

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation AnimalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.

    private void Start()
    {
        //StartCoroutine(OnAttackRange());
        maxHp = hp;
    }

    private void Update()
    {
        if (isContact)
        {
            Vector2 stop = new Vector2(0, 0);
            Move(stop);
            AnimalAnimation.Jump();
        }
        else
        {
            Vector2 go = new Vector2(-1f, 0f);
            Move(go);
            AnimalAnimation.Walk();
        }
        //Attack();
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }

    public bool IsContact()
    {
        isContact = !isContact;
        return isContact;
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
