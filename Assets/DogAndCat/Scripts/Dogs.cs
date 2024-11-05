using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dogs : MonoBehaviour
{
    public float moveSpeed = 0.5f; //이동속도
    public Vector2 moveDir; //움직이는 방향
    public float Scale = 0.6f; //크기

    public float attackRange_X = 1f; //X축 공격 범위
    public float attackRange_Y = 1f; //Y축 공격 범위

    public float hp = 50; //체력
    private float maxHp; //최대 체력
    public float damage = 8; //공격력
    public float costValue = 200; //생산비용

    public bool isDead = false; //죽었는가?

    public bool isContact = false; //적을 만났는가?

    private Collider2D[] detectedEnemies = null; //만난 적을 담아둘 리스트

    private float startAttackTime; //공격을 시작한 시간
    public float attackInterval = 1f; //공격주기

    public bool isRangeAttackType; //체크하면 광역공격, 아니면 단일공격

    private float hpBarAmount { get { return hp / maxHp; } } 
    
    public Image hpBar;

    public bool drawGizmos;

    public LayerMask TargetLayer;

    [Tooltip("자식에 있는 오브젝트를 넣어주세요.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //처음엔 위에처럼 작성했는데 참조가 되지 않아서 계속 오류가 나왔다. 그래서 그냥 CharacterAnimation에서 자식 컴포넌트에 있는
    //Animator을 직접 참조하게만 하고 나는 public으로 Animator가 있는 자식 오브젝트를 직접 참조하게 해주었다.

    private void Start()
    {
        //StartCoroutine(OnAttackRange());
        maxHp = hp;
        //ResourcesManager에서 GameManager에 dogs를 담아줄거라 아래꺼는 취소
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
        //안만나면 계속 왼쪽으로 감
        if (!isContact)
        {
            moveDir = Vector2.left;
        }
        //만나면 벡터값 0
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
        //중심은 x좌표는 (공격 범위 / 2) 만큼 왼쪽으로 가고, y좌표는 그대로이다. 그리고 상자를 그려준다.
        Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - (attackRange_X / 2),transform.position.y)
            , new Vector2(attackRange_X, attackRange_Y), 0, TargetLayer);

        //그냥 detectedEnemy = enemyColldiers해버리면 얕은 복사가 되어버리니까 이렇게 해줘야 한다
        detectedEnemies = new Collider2D[enemyColliders.Length]; //배열 크기 선언
        Array.Copy(enemyColliders, detectedEnemies, enemyColliders.Length);

        foreach (var enemyCollider in enemyColliders)
        {
            if (enemyCollider.CompareTag("Enemy") && enemyCollider != null)
            {
                //Debug.Log("적이 감지됨");
                //적이 감지되면 멈춰야함 멈추고 나서 공격을 하던 말던 결정해야한다.
                isContact = true;
                break; //적 찾으면 확인 안함
            }

        }
    }


    //움직임, moveDir이 0일때 0이 아닐때를 구분
    public void Move(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0)
        {
            transform.localScale = new Vector3(Scale, Scale, Scale);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            animalAnimation.Walk();
        }
        //moveDir.magnitude가 0이면 적을 만났다는거니까 공격을 계시. 근데 그 전에 공격 타입을 따져봐야함
        else
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    public void CheckAttackType(bool isRangeAttackType)
    {
        if (isRangeAttackType)
        {
            print("범위공격");
            OnRangeAttack();
        }

        else
        {
            print("단일공격");
            OnDirectAttacK();
        }
    }

    //범위 공격
    public void OnRangeAttack()
    {
        //살아있는 적을 저장할 리스트
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
                        aliveEnemies.Add(detectedEnemy); //살아있는 적만 추가
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
        //감지된 적이 0보다 크면 true, 아니면 false
        isContact = detectedEnemies.Length > 0;
    }

    //단일 공격
    public void OnDirectAttacK()
    {
        Collider2D closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var detectedEnemy in detectedEnemies)
        {
            if (detectedEnemy != null && detectedEnemy.CompareTag("Enemy"))
            {
                //타겟들과 dog와의 거리를 계산
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
        print("공격받음");
        if (hp <= 0)
        {
            print("죽음");
            hp = 0;
            isDead = true;
            OnDead();

        }
    }

    //죽었는지 살았는지 확인하는 함수
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
    //죽으면 뜨는 함수
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

    ////적 콜라이더를 감지함
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
    //                print($"지금 범위에 있는 고양이 수 : {contactedColls.Length}");
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
    //    //        print($"지금 범위에 있는 고양이 수 : {contactedColls.Length}");
    //    //    }
    //    //}
    //    animalAnimation.Jump();
    //}

}
