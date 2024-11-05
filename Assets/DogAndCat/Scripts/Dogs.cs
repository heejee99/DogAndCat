using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //�̵��ӵ�
    public Vector2 moveDir; //�����̴� ����
    public float Scale = 0.6f; //ũ��

    public float attackRange_X = 1f; //X�� ���� ����
    public float attackRange_Y = 1f; //Y�� ���� ����

    public float hp = 50; //ü��
    private float maxHp; //�ִ� ü��
    public float damage = 8; //���ݷ�
    public float costValue = 200; //������

    public bool isDead = false; //�׾��°�?

    public bool isContact = false; //���� �����°�?

    private Collider2D[] detectedEnemies = null; //���� ���� ��Ƶ� ����Ʈ

    private float startAttackTime; //������ ������ �ð�
    public float attackInterval = 1f; //�����ֱ�

    public bool isRangeAttackType; //üũ�ϸ� ��������, �ƴϸ� ���ϰ���

    private float hpBarAmount { get { return hp / maxHp; } } 
    
    public Image hpBar;

    public bool drawGizmos;

    public LayerMask TargetLayer;

    [Tooltip("�ڽĿ� �ִ� ������Ʈ�� �־��ּ���.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //ó���� ����ó�� �ۼ��ߴµ� ������ ���� �ʾƼ� ��� ������ ���Դ�. �׷��� �׳� CharacterAnimation���� �ڽ� ������Ʈ�� �ִ�
    //Animator�� ���� �����ϰԸ� �ϰ� ���� public���� Animator�� �ִ� �ڽ� ������Ʈ�� ���� �����ϰ� ���־���.

    private void Start()
    {
        //StartCoroutine(OnAttackRange());
        maxHp = hp;
        //ResourcesManager���� GameManager�� dogs�� ����ٰŶ� �Ʒ����� ���
        //GameManager.Instance.dogs.Add(this);
    }

    private void Update()
    {
        //if (isContact)
        //{
        //    if (hp > 0)
        //    {
        //        OnAttack();
        //    }
        //    else 
        //    {
        //        OnDead();
        //    }
        //    //Attack();
        //}
        //else
        //{
        //    Vector2 gLeft = Vector2.left;
        //    Move(gLeft);
        //    animalAnimation.Walk();
        //}
        CheckEnemy();

        if (isDead)
        {
            return;
        }
        //�ȸ����� ��� �������� ��
        if (!isContact)
        {
            moveDir = Vector2.left;
        }
        //������ ���Ͱ� 0
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



        hpBar.fillAmount = hpBarAmount;
    }

    public void CheckEnemy()
    {
        //�߽��� x��ǥ�� (���� ���� / 2) ��ŭ �������� ����, y��ǥ�� �״���̴�. �׸��� ���ڸ� �׷��ش�.
        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - (attackRange_X / 2),transform.position.y)
            , new Vector2(attackRange_X, attackRange_Y), 0, TargetLayer);

        //�׳� detectedEnemy = enemyColldiers�ع����� ���� ���簡 �Ǿ�����ϱ� �̷��� ����� �Ѵ�
        detectedEnemies = new Collider2D[enemyColliders.Length]; //�迭 ũ�� ����
        Array.Copy(enemyColliders, detectedEnemies, enemyColliders.Length);

        foreach (var enemyCollider in enemyColliders)
        {
            if (enemyCollider.CompareTag("Enemy") && enemyCollider != null)
            {
                //Debug.Log("���� ������");
                //���� �����Ǹ� ������� ���߰� ���� ������ �ϴ� ���� �����ؾ��Ѵ�.
                isContact = true;
                break; //�� ã���� Ȯ�� ����
            }

        }
    }


    //������, moveDir�� 0�϶� 0�� �ƴҶ��� ����
    public void Move(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0)
        {
            transform.localScale = new Vector3(Scale, Scale, Scale);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            animalAnimation.Walk();
        }
        //moveDir.magnitude�� 0�̸� ���� �����ٴ°Ŵϱ� ������ ���. �ٵ� �� ���� ���� Ÿ���� ����������
        else
        {
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

    //���� ����
    public void OnRangeAttack()
    {
        //����ִ� ���� ������ ����Ʈ
        List<Collider2D> aliveEnemies = new List<Collider2D>();

        foreach (var detectedEnemy in detectedEnemies)
        {
            if (detectedEnemy != null && detectedEnemy.CompareTag("Enemy"))
            {
                if (detectedEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                    if (!enemy.isDead)
                    {
                        aliveEnemies.Add(detectedEnemy); //����ִ� ���� �߰�
                    }
                }

                else if (detectedEnemy.TryGetComponent<Cats>(out Cats cat))
                {
                    cat.TakeDamage(damage);
                    if (!cat.isDead)
                    {
                        aliveEnemies.Add(detectedEnemy);
                    }

                }
            }
        }

        detectedEnemies = aliveEnemies.ToArray();
        //������ ���� 0���� ũ�� true, �ƴϸ� false
        isContact = detectedEnemies.Length > 0;
    }

    //���� ����
    public void OnDirectAttacK()
    {
        Collider2D closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var detectedEnemy in detectedEnemies)
        {
            if (detectedEnemy != null && detectedEnemy.CompareTag("Enemy"))
            {
                //Ÿ�ٵ�� dog���� �Ÿ��� ���
                float targetDistance = (detectedEnemy.transform.position - transform.position).magnitude;

                if (targetDistance < closestDistance)
                {
                    closestDistance = targetDistance;
                    closestEnemy = detectedEnemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (closestEnemy.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                if (!enemy.isDead)
                {
                    detectedEnemies = detectedEnemies.Where(e => e != closestEnemy).ToArray();
                }

            }
            else if (closestEnemy.TryGetComponent<Cats>(out Cats cat))
            {
                cat.TakeDamage(damage);
                if (!cat.isDead)
                {
                    detectedEnemies = detectedEnemies.Where(e => e != closestEnemy).ToArray();
                }

            }

            isContact = detectedEnemies.Length > 0;
            print(closestEnemy.name);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        print("���ݹ���");
        if (hp <= 0)
        {
            print("����");
            hp = 0;
            isDead = true;
            OnDead();

        }
    }

    //�׾����� ��Ҵ��� Ȯ���ϴ� �Լ�
    //public bool IsDead()
    //{
    //    if (hp == -1)
    //    {
    //        isDead = true;
    //        OnDead(isDead);
    //    }

    //    return isDead;
    //}

    public GameObject deathParticlePrefab;
    //������ �ߴ� �Լ�
    public void OnDead()
    {
        if (isDead)
        {
            //GameObject deathParticle = Instantiate(deathParticlePrefab, transform);
            //deathParticle.transform.localPosition = Vector3.zero;
            //deathParticle.transform.localRotation = Quaternion.identity;
            animalAnimation.Sleep();
            StartCoroutine(DeleteDogObject());
        }
    }

    IEnumerator DeleteDogObject()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.dogs.Remove(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireCube(new Vector2(transform.position.x - (attackRange_X / 2),
            transform.position.y), new Vector2(attackRange_X, attackRange_Y));
            Gizmos.color = Color.yellow;
        }
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

    //public void OnAttackRange()
    //{
    //    while (true)
    //    {
    //        if ()
    //        Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //        foreach (Collider2D contactedColl in contactedColls)
    //        {
    //            if (contactedColl.CompareTag("Cat"))
    //            {
    //                print($"���� ������ �ִ� ����� �� : {contactedColls.Length}");
    //            }
    //        }
    //    }
    //}

    //public void Attack()
    //{
    //    //Collider2D[] contactedColls = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 1 * attackRange, transform.position.y), new Vector2(1, 1) * attackRange / 2, 0);

    //    //foreach (Collider2D contactedColl in contactedColls)
    //    //{
    //    //    if (contactedColl.CompareTag("Cat"))
    //    //    {
    //    //        print($"���� ������ �ִ� ����� �� : {contactedColls.Length}");
    //    //    }
    //    //}
    //    animalAnimation.Jump();
    //}

}
