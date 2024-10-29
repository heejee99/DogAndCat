using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public float Scale = 0.6f; //��� ũ��

    private bool isContact = false; //���� �����°�?

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation AnimalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.

    private void Start()
    {
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
            Vector2 moveDir = new Vector2(-1f, 0f);
            Move(moveDir);
            AnimalAnimation.Walk();
        }
    }

    //������
    public void Move(Vector2 moveDir)
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }

    //�� �ݶ��̴��� ������
    public void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.TryGetComponent<Cats>(out Cats cats))
        {
            isContact = true; 
        }
    }

}
