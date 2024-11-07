using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class Cat : Creature, IHealth
{
    public float hpBarAmount { get { return hp / maxHp; } }
    private void Start()
    {
        base.Start();
        GameManager.Instance.cat.Add(this);
    }

    private void Update()
    {
        base.Update();
        
    }

    protected override void CheckEnemy()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackRange_X / 2), transform.position.y)
            , new Vector2(attackRange_X, attackRange_Y), 0, TargetLayer);
        //���� OverlapBoxAll�� �ɷ��� Collider�� �޾��� Collider2D����Ʈ ũ�� �ʱ�ȭ
        detectedEnemies = new Collider2D[enemyColliders.Length];
        //�迭 �ʱ�ȭ
        Array.Copy(enemyColliders, detectedEnemies, enemyColliders.Length);
    }

    //�����ϴ� ����
    protected override void DoAttackAnimaion()
    {
        //������ �ݶ��̴��� �ƿ� ������ �׳� �̵�
        if (detectedEnemies.Length == 0)
        {
            print("������ ���� ���� from cat");
            //������ ���� ������ isMove�� true
            //isMove = true;
            return;
        }
        //1�ʸ��� ������ �Լ��ε� ������ ���� ����
        foreach (Collider2D detectedEnemy in detectedEnemies)
        {
            print($"������ �������� ���� : {detectedEnemies.Length}");
            //Enemy �±׸� ã����
            if (detectedEnemy.CompareTag("Player"))
            {
                print("�������� ã��");
                //������ ���߰�
                //isMove = false;
                //���� �ִϸ��̼� ���
                animation.SetTrigger("onJumping");
                //animation.Jump();
                //���� �������� �Ʒ� OnRangeAttack�� OnDirectAttack�� Unity
                //�ִϸ��̼� �̺�Ʈ�� ����
            }
        }
    }

    
    //public override void OnRangeAttack()
    //{
    //    //if (detectedEnemies == null)
    //    //{
    //    //    return;
    //    //}
    //    //foreach (Collider2D detectedEnemy in detectedEnemies)
    //    //{
    //    //    if (detectedEnemy.TryGetComponent<Dog>(out Dog dog))
    //    //    {
    //    //        dog.TakeDamage(damage);
    //    //    }
    //    //    if (detectedEnemy.TryGetComponent<Player>(out Player player))
    //    //    {
    //    //        player.TakeDamage(damage); 
    //    //    }
    //    //}
    //    if (detectedEnemies != null)
    //    {
    //        foreach (Collider2D detectedEnemy in detectedEnemies)
    //        {
    //            if (detectedEnemy.TryGetComponent<Dog>(out Dog dog))
    //            {
    //                dog.TakeDamage(damage);
    //            }
    //            if (detectedEnemy.TryGetComponent<Player>(out Player player))
    //            {
    //                player.TakeDamage(damage);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        print("�����Ǵ� �������� ����");
    //    }
    //}

    protected override void OnDirectAttack()
    {
    }

    protected override void OnDrawGizmo()
    {

    }

    protected override void OnDead()
    {
        //base.OnDead();
        isDead = true;
        isMove = false;
        boxCollider2D.enabled =false;
        animation.SetBool("isDead", true);
        //animation.Sleep();
        print("Cat������ƮOnDead����");
        StartCoroutine(DeadCoroutine());
    }

    public void TakeDamage(float damage)
    {
        //�� ������Ʈ�� ������ �������� ���� ����
        if (isDead == true)
        {
            return;
        }
        //ü���� 0���� Ŭ���� �������� ����
        if (hp > 0)
        {
            hp -= damage;
            print("�������� ����");
            if (hp <= 0)
            {
                hp = 0;
                OnDead();
            }
        }
        //���� TakeDamage�� �����ߴµ� ü���� 0���� ũ�� �ʴٸ� OnDead()�� ����
        //else
        //{
        //    hp = 0;
        //    OnDead();
        //}
    }

    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.cat.Remove(this);
        Destroy(gameObject);
    }
}
