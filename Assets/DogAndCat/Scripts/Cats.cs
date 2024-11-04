using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using System.Runtime.InteropServices;
//using System.Xml.Serialization;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Cats : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public Vector2 moveDir; //�����̴� ����
    public float Scale = 0.6f; //ũ��

    public float attackRange_X = 1f; //X�� ���� ����
    public float attackRange_Y = 1f; //Y�� ���� ����

    public float hp = 80; //ü��
    private float maxHp; //�ִ� ü��
    public float damage = 6; //���ݷ�

    public bool isDead = false; //�׾��°�?

    public bool isContact = false; //�÷��̾ �����°�?

    private Collider2D[] detectedPlayers = null;

    private float startAttackTime; //������ ������ �ð�
    public float attackInterval = 1f; //�����ֱ�

    public bool isRangeAttackType; //üũ�ϸ� ��������, �ƴϸ� ���ϰ���

    //ü�¹ٴ� Cat���� �پ������ϱ� �������� �����ϴ°ɷ� ����
    private float hpBarAmount { get { return hp / maxHp;  } }

    public Image hpBar;

    public bool drawGizmos;

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.
   
    private void Start()
    {
        maxHp = hp;
    }

    private void Update()
    {
        CheckEnemy();

        if (!isDead)
        {
            if (!isContact)
            {
                moveDir = Vector2.right;
            }
            else
            {
                moveDir = Vector2.zero;
                animalAnimation.Jump();
            }

            Move(moveDir);

            if (Time.time > startAttackTime + attackInterval && isContact)
            {
                startAttackTime = Time.time;
                CheckAttackType(isRangeAttackType);
            }
        }

        else
        {
            OnDead();
        }

        hpBar.fillAmount = hpBarAmount;
    }

    //�� ����
    public void CheckEnemy()
    {
        Collider2D[] playerColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackRange_X / 2), transform.position.y)
            , new Vector2(attackRange_X, attackRange_Y), 0);

        detectedPlayers = new Collider2D[playerColliders.Length]; //�迭 ũ�� �ʱ�ȭ
        Array.Copy(playerColliders, detectedPlayers, playerColliders.Length);

        isContact = false; //�⺻���� false�� �ʱ�ȭ

        foreach (var playerCollider in playerColliders)
        {
            if (playerCollider.CompareTag("Player") && playerCollider != null)
            {
                isContact = true;
                break; //�� ã���� �Լ� out
            }
        }

    }

    //������, moveDir�� 0�϶� 0�� �ƴҶ��� ����
    public void Move(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0)
        {
            //Sprite�� ������� �ϹǷ� -Scale
            transform.localScale = new Vector3(-Scale, Scale, Scale);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            animalAnimation.Walk();
        }
        else
        {
            transform.localScale = new Vector3(-Scale, Scale, Scale);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    public void CheckAttackType(bool isRangeAttackType)
    {
        if (isRangeAttackType)
        {
            print("��������");
            OnRangeAttack();
        }

        else
        {
            print("���ϰ���");
            OnDirectAttacK();
        }
    }

    public void OnRangeAttack()
    {
        //����ִ� �÷��̾ ������ ����Ʈ
        List<Collider2D> aliveplayers = new List<Collider2D>();

        foreach (var detectedPlayer in detectedPlayers)
        {
            if (detectedPlayer != null && detectedPlayer.CompareTag("Player"))
            {
                if (detectedPlayer.TryGetComponent<Player>(out Player player))
                {
                    player.TakeDamage(damage);
                    if (!player.isDead)
                    {
                        aliveplayers.Add(detectedPlayer);
                    }
                }

                else if (detectedPlayer.TryGetComponent<Dogs>(out Dogs dog))
                {
                    dog.TakeDamage(damage);
                    if (!dog.isDead)
                    {
                        aliveplayers.Add(detectedPlayer);
                    }
                }
            }
        }

        //������ ���� �ٽ� �ʱ�ȭ
        detectedPlayers = aliveplayers.ToArray();
        //������ ���� 0���� ũ�� true, �ƴϸ� false
        isContact = detectedPlayers.Length > 0;
    }

    public void OnDirectAttacK()
    {
        Collider2D closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var detectedPlayer in detectedPlayers)
        {
            if (detectedPlayer != null && detectedPlayer.CompareTag("Player"))
            {
                float targetDistance = (detectedPlayer.transform.position - transform.position).magnitude;
                
                if (targetDistance < closestDistance)
                {
                    closestDistance = targetDistance;
                    closestPlayer = detectedPlayer;
                }
            }
        }

        if (closestPlayer != null)
        {
            if (closestPlayer.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(damage);
                if (!player.isDead)
                {
                    detectedPlayers = detectedPlayers.Where(e => e != closestPlayer).ToArray();
                }
            }
            //���� ������ ��¥�� �Ѹ� �����ϱ� ������ ���� ���ݰ� �ٸ��� else if�� ��
            else if (closestPlayer.TryGetComponent<Dogs>(out Dogs dog))
            {
                dog.TakeDamage(damage);
                if (!dog.isDead)
                {
                    detectedPlayers = detectedPlayers.Where(e => e != closestPlayer).ToArray();
                }
            }

            isContact = detectedPlayers.Length > 0;
            print(closestPlayer.name);
        }
    }

    //������ ����
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
        
    }

    //public bool IsDead()
    //{
    //    if (hp <= -1)
    //    {
    //        isDead = true;
    //        OnDead(isDead);
    //    }
    //    return isDead;
    //}

    public void OnDead()
    {
        if (isDead)
        {
            animalAnimation.Sleep();
            StartCoroutine(DeleteCatObject());
        }
    }

    IEnumerator DeleteCatObject()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.cats.Remove(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireCube(new Vector2(transform.position.x + (attackRange_X / 2), transform.position.y)
                , new Vector2(attackRange_X, attackRange_Y));
            Gizmos.color = Color.yellow;
        }
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
