using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //ũ��
    public float attackInterval = 1f; //���ݼӵ�
    public float hp = 80; //ü��
    private float maxHp; //�ִ� ü��
    public float damage = 6; //���ݷ�
    public bool attackType; //���� Ÿ��

    public bool isDead = false; //�׾�����


    public bool isContact = false;

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.
    private void Awake()
    {

    }
    private void Start()
    {
        maxHp = hp;
    }

    private void Update()
    {
        if (isContact)
        {
            OnAttack();
            //Attack();
        }
        else
        {
            Vector2 goRight = Vector2.right;
            Move(goRight);
            animalAnimation.Walk();
        }

        IsDead();
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(-Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    public void OnAttack()
    {
        Move(Vector2.zero);
        animalAnimation.Jump();
    }

    //������ ����
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (IsDead())
        {
            OnDead();
        }
    }

    public bool IsDead()
    {
        if (hp <= 0)
        {
            isDead = true;
        }
        return isDead;
    }

    private void OnDead()
    {
        GameManager.Instance.cats.Remove(this);
        Destroy(gameObject);
    }

    //�� �ݶ��̴��� ������
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Dogs>(out Dogs dogs))
    //    {
    //        isContact = true;
    //    }
    //}
}
