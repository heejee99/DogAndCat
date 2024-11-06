using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Cat�� Dog�� ����� ��ũ��Ʈ
public abstract class Creature : MonoBehaviour
{
    public float moveSpeed; //�̵��ӵ�
    public bool goLeft; //üũ�ϸ� ��������, �ƴϸ� ���������� �̵���
    public float Scale = 0.6f; //ũ��

    public float attackRange_X; //X�� ���ݹ���
    public float attackRange_Y; //Y�� ���ݹ���

    public float hp; //ü��
    public float maxHp; //�ִ�ü��
    public float damage; //���ݷ�

    private bool isDead = false; //�׾��°�?

    private bool isContact = false; //���� �����°�?
    private Collider2D[] detectedEnemies = null; //���� ��

    private float startAttackTime; //������ ������ �ð�
    public float attackInterval = 1f; //���� �ֱ�

    public bool isRangeAttackType; //����Ÿ��. true -> ��������, false -> ���ϰ���

    private float hpBarAmount { get { return hp / maxHp; } } //ü�� ����

    public Image hpBar; //hp�� �̹���

    public LayerMask TargetLayer; //Ÿ���� ���̾ �� �����ؾ���

    private AnimalAnimation animation;

    protected void Awake()
    {
        animation = GetComponentInChildren<AnimalAnimation>();
    }
    protected void Start()
    {
        maxHp = hp;
    }

    protected void Update()
    {
        CheckEnemy();
    }

    //üũ�ϴ� �߻��Լ� -> �ڽĿ��� ������ �ؾ���
    protected abstract Collider2D[] CheckEnemy();

    protected void Move()
    {
        //�������� ���°� ����������
        if (goLeft)
        {
            //ũ�� ����
            transform.localScale = Vector3.one * Scale;
            //�̵� �� ���� ����
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            
        }
    }
}
