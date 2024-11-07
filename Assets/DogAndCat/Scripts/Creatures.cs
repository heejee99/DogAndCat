using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Cat과 Dog를 상속할 스크립트
public abstract class Creature : MonoBehaviour
{
    public float moveSpeed; //이동속도
    public bool isMove; //이동 여부
    public bool goLeft; //켜놓으면 Left, 꺼놓으면 Right로 이동
    private Vector2 moveDir; //이동 방향

    public float scale = 0.6f; //크기

    public float attackRange_X; //X축 공격범위
    public float attackRange_Y; //Y축 공격범위

    public float hp; //체력
    protected float maxHp; //최대체력
    public float damage; //공격력

    public bool isDead = false; //죽었는가?

    private bool isContact = false; //적을 만났는가?
    public Collider2D[] detectedEnemies = null; //만난 적

    private float startAttackTime; //공격을 시작한 시간
    public float attackInterval = 1f; //공격 주기

    public bool isRangeAttackType; //공격타입. true -> 범위공격, false -> 단일공격

    public float hpBarAmount { get { return hp / maxHp; } } //체력 비율

    public Image hpBar; //hp바 이미지

    public bool drawGizmo; //켜면 기즈모 그릴 수 있음

    public LayerMask TargetLayer; //타겟의 레이어를 꼭 지정해야함

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
        //왼쪽으로 가기가 켜져있으면 왼쪽, 아니면 오른쪽, 스케일도 조절한다.
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

            //계속 공격할껀지 탐색 근데 1초마다 한번씩
            if (Time.time > startAttackTime + attackInterval && detectedEnemies != null)
            {
                //공격하는데, 이건 공격 모션시작이랑, Move를 멈춰주는 역할
                DoAttackAnimaion();
                startAttackTime = Time.time;
            }
            else if (detectedEnemies == null)
            {
                print("감지된 적 없음");
            }
        }

        hpBar.fillAmount = hpBarAmount;
    }

    //체크하는 추상함수 -> 자식에서 구현을 해야함
    protected abstract void CheckEnemy();

    protected void Move()
    {
        //이동이 켜져있으면 이동하기, Walk애니메이션 재생, 실제 이동
        //if (isMove)
        //{
            //이동 및 방향 조절
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
    //    //이 오브젝트가 죽으면 데미지를 받지 않음
    //    if (isDead == true)
    //    {
    //        return;
    //    }
    //    //체력이 0보다 클때만 데미지를 받음
    //    if (hp > 0)
    //    {
    //        hp -= damage;
    //        print("데미지를 받음");
    //        if (hp <= 0)
    //        {
    //            hp = 0;
    //            OnDead();
    //        }
    //    }
    //    //만약 TakeDamage를 실행했는데 체력이 0보다 크지 않다면 OnDead()를 실행
    //    //else
    //    //{
    //    //    hp = 0;
    //    //    OnDead();
    //    //}
    //}

    protected abstract void OnDead();


    protected abstract void OnDrawGizmo();
}
