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
    public float moveSpeed = 0.5f; //이동속도
    public Vector2 moveDir; //움직이는 방향
    public float Scale = 0.6f; //크기

    public float attackRange_X = 1f; //X축 공격 범위
    public float attackRange_Y = 1f; //Y축 공격 범위

    public float hp = 80; //체력
    private float maxHp; //최대 체력
    public float damage = 6; //공격력

    public bool isDead = false; //죽었는가?

    public bool isContact = false; //플레이어를 만났는가?

    private Collider2D[] detectedPlayers = null;

    private float startAttackTime; //공격을 시작한 시간
    public float attackInterval = 1f; //공격주기

    public bool isRangeAttackType; //체크하면 광역공격, 아니면 단일공격

    //체력바는 Cat에게 붙어있으니까 개개인이 관리하는걸로 하자
    private float hpBarAmount { get { return hp / maxHp;  } }

    public Image hpBar;

    public bool drawGizmos;

    [Tooltip("자식에 있는 오브젝트를 넣어주세요.")]
    public AnimalAnimation animalAnimation;
    //AnimalAnimation AnimalAnimation = new AnimalAnimation();
    //처음엔 위에처럼 작성했는데 참조가 되지 않아서 계속 오류가 나왔다. 그래서 그냥 CharacterAnimation에서 자식 컴포넌트에 있는
    //Animator을 직접 참조하게만 하고 나는 public으로 Animator가 있는 자식 오브젝트를 직접 참조하게 해주었다.
   
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

    //적 감지
    public void CheckEnemy()
    {
        Collider2D[] playerColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackRange_X / 2), transform.position.y)
            , new Vector2(attackRange_X, attackRange_Y), 0);

        detectedPlayers = new Collider2D[playerColliders.Length]; //배열 크기 초기화
        Array.Copy(playerColliders, detectedPlayers, playerColliders.Length);

        isContact = false; //기본값은 false로 초기화

        foreach (var playerCollider in playerColliders)
        {
            if (playerCollider.CompareTag("Player") && playerCollider != null)
            {
                isContact = true;
                break; //적 찾고나면 함수 out
            }
        }

    }

    //움직임, moveDir이 0일때 0이 아닐때를 구분
    public void Move(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0)
        {
            //Sprite를 뒤집어야 하므로 -Scale
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
            print("범위공격");
            OnRangeAttack();
        }

        else
        {
            print("단일공격");
            OnDirectAttacK();
        }
    }

    public void OnRangeAttack()
    {
        //살아있는 플레이어를 저장할 리스트
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

        //감지된 적을 다시 초기화
        detectedPlayers = aliveplayers.ToArray();
        //감지된 적이 0보다 크면 true, 아니면 false
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
            //단일 공격은 어짜피 한명만 공격하기 때문에 범위 공격과 다르게 else if를 씀
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

    //데미지 입음
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
    //적 콜라이더를 감지함
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent<Dogs>(out Dogs dogs))
    //    {
    //        isContact = true;
    //    }
    //}
}
