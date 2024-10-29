using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //ũ��
    public float hp = 80; //ü��
    private float maxHp; //�ִ� ü��
    public float attack = 6; //���ݷ�


    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation AnimalAnimation;
    private bool isContact = false;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.
    private void Awake()
    {

        GameManager.Instance.cats.Add(this);
    }
    private void Start()
    {
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
            Vector2 go = new Vector2(1f, 0f);
            Move(go);
            AnimalAnimation.Walk();
        }
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(-Scale, Scale, Scale);
    }

    //������ ����
    public void TakeDamage(float damage)
    {
        print("����̰� �������� ����");
        hp -= damage;
    }
    

    //�� �ݶ��̴��� ������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Dogs>(out Dogs dogs))
        {
            isContact = true;
        }
    }
}
