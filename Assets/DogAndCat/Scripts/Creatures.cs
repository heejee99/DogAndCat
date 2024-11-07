using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Cat�� Dog�� ����� ��ũ��Ʈ
public abstract class Creature : MonoBehaviour
{
    public float moveSpeed; //�̵��ӵ�
    public bool isMove; //�̵� ����
    public bool goLeft; //�ѳ����� Left, �������� Right�� �̵�
    private Vector2 moveDir; //�̵� ����

    public float scale = 0.6f; //ũ��

    public float attackRange_X; //X�� ���ݹ���
    public float attackRange_Y; //Y�� ���ݹ���

    public float hp; //ü��
    protected float maxHp; //�ִ�ü��
    public float damage; //���ݷ�

    public bool isDead = false; //�׾��°�?

    private bool isContact = false; //���� �����°�?
    public Collider2D[] detectedEnemies = null; //���� ��

    private float startAttackTime; //������ ������ �ð�
    public float attackInterval = 1f; //���� �ֱ�

    public bool isRangeAttackType; //����Ÿ��. true -> ��������, false -> ���ϰ���

    public float hpBarAmount { get { return hp / maxHp; } } //ü�� ����

    public Image hpBar; //hp�� �̹���

    public bool drawGizmo; //�Ѹ� ����� �׸� �� ����

    public LayerMask TargetLayer; //Ÿ���� ���̾ �� �����ؾ���

    public Animator animation;

    protected BoxCollider2D boxCollider2D;

    protected void Awake()
    {
        animation = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }
    protected void Start()
    {
        maxHp = hp;
        //�������� ���Ⱑ ���������� ����, �ƴϸ� ������, �����ϵ� �����Ѵ�.
        if (goLeft)
        {
            moveDir = Vector2.left;
            transform.localScale = Vector3.one * scale;
        }
        else
        {
            moveDir = Vector2.right;
            transform.localScale = new Vector3(-scale, scale, scale);
        }
    }

    protected void Update()
    {
        if (isDead == false)
        {
            CheckEnemy();
            if (detectedEnemies.Length > 0)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            if (isMove == true)
            {
                Move();
            }
            else
            {

                transform.Translate(Vector2.zero * moveSpeed * Time.deltaTime);
                animation.SetBool("isWalking", false);
            }

            //��� �����Ҳ��� Ž�� �ٵ� 1�ʸ��� �ѹ���
            if (Time.time > startAttackTime + attackInterval && detectedEnemies != null)
            {
                //�����ϴµ�, �̰� ���� ��ǽ����̶�, Move�� �����ִ� ����
                DoAttackAnimaion();
                startAttackTime = Time.time;
            }
            else if (detectedEnemies == null)
            {
                print("������ �� ����");
            }
        }

        hpBar.fillAmount = hpBarAmount;
    }

    //üũ�ϴ� �߻��Լ� -> �ڽĿ��� ������ �ؾ���
    protected abstract void CheckEnemy();

    protected void Move()
    {
        //�̵��� ���������� �̵��ϱ�, Walk�ִϸ��̼� ���, ���� �̵�
        //if (isMove)
        //{
            //�̵� �� ���� ����
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            animation.SetBool("isWalking", true);
            //animation.Walk();
        //}
        //else
        //{
        //    animation.SetBool("isWalking", false);
        //    //animation.Idle();
        //}
    }

    protected abstract void DoAttackAnimaion();

    //public abstract void OnRangeAttack();
    protected abstract void OnDirectAttack();

    //public void TakeDamage(float damage)
    //{
    //    //�� ������Ʈ�� ������ �������� ���� ����
    //    if (isDead == true)
    //    {
    //        return;
    //    }
    //    //ü���� 0���� Ŭ���� �������� ����
    //    if (hp > 0)
    //    {
    //        hp -= damage;
    //        print("�������� ����");
    //        if (hp <= 0)
    //        {
    //            hp = 0;
    //            OnDead();
    //        }
    //    }
    //    //���� TakeDamage�� �����ߴµ� ü���� 0���� ũ�� �ʴٸ� OnDead()�� ����
    //    //else
    //    //{
    //    //    hp = 0;
    //    //    OnDead();
    //    //}
    //}

    protected abstract void OnDead();


    protected abstract void OnDrawGizmo();
}
