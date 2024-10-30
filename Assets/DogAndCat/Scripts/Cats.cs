using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //ũ��
    public float attackSpeed = 1f; //���ݼӵ�
    //�����Ÿ��� ���� �ݶ��̴��� �����ؼ� ���� ����
    public float hp = 80; //ü��
    private float maxHp; //�ִ� ü��
    public float damage = 6; //���ݷ�
    public bool attackType; //���� Ÿ��
    public bool isDead = false; //�׾�����



    public bool isContact = false;

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation AnimalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.
    private void Awake()
    {

    }
    private void Start()
    {
        maxHp = hp;
        GameManager.Instance.cats.Add(this);
    }

    private void Update()
    {
        if (isContact)
        {
            Vector2 stop = Vector2.zero;
            Move(stop);
            AnimalAnimation.Jump();
        }
        else
        {
            Vector2 goRight = Vector2.right;
            Move(goRight);
            AnimalAnimation.Walk();
        }

        IsDead();
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(-Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    //������ ����
    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public bool IsContact()
    {
        isContact = !isContact;
        return isContact;
    }

    public bool IsDead()
    {
        if (hp <= 0)
        {
            isDead = true;
        }
        DestroyImmediate(this);
        return isDead;
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
