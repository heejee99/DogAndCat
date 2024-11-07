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
        //위에 OverlapBoxAll로 걸러준 Collider를 받아줄 Collider2D리스트 크기 초기화
        detectedEnemies = new Collider2D[enemyColliders.Length];
        //배열 초기화
        Array.Copy(enemyColliders, detectedEnemies, enemyColliders.Length);
    }

    //공격하는 조건
    protected override void DoAttackAnimaion()
    {
        //감지된 콜라이더가 아예 없으면 그냥 이동
        if (detectedEnemies.Length == 0)
        {
            print("감지된 적이 없음 from cat");
            //감지된 적이 없으면 isMove가 true
            //isMove = true;
            return;
        }
        //1초마다 들어오는 함수인데 감지된 적을 돌고
        foreach (Collider2D detectedEnemy in detectedEnemies)
        {
            print($"감지된 강아지의 숫자 : {detectedEnemies.Length}");
            //Enemy 태그를 찾으면
            if (detectedEnemy.CompareTag("Player"))
            {
                print("강아지를 찾음");
                //움직임 멈추고
                //isMove = false;
                //점프 애니메이션 재생
                animation.SetTrigger("onJumping");
                //animation.Jump();
                //실제 데미지는 아래 OnRangeAttack과 OnDirectAttack를 Unity
                //애니메이션 이벤트로 참조
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
    //        print("감지되는 강아지가 없음");
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
        print("Cat컴포넌트OnDead실행");
        StartCoroutine(DeadCoroutine());
    }

    public void TakeDamage(float damage)
    {
        //이 오브젝트가 죽으면 데미지를 받지 않음
        if (isDead == true)
        {
            return;
        }
        //체력이 0보다 클때만 데미지를 받음
        if (hp > 0)
        {
            hp -= damage;
            print("데미지를 받음");
            if (hp <= 0)
            {
                hp = 0;
                OnDead();
            }
        }
        //만약 TakeDamage를 실행했는데 체력이 0보다 크지 않다면 OnDead()를 실행
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
