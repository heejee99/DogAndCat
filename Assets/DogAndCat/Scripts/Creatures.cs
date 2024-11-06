using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Cat과 Dog를 상속할 스크립트
public abstract class Creature : MonoBehaviour
{
    public float moveSpeed; //이동속도
    public bool goLeft; //체크하면 왼쪽으로, 아니면 오른쪽으로 이동함
    public float Scale = 0.6f; //크기

    public float attackRange_X; //X축 공격범위
    public float attackRange_Y; //Y축 공격범위

    public float hp; //체력
    public float maxHp; //최대체력
    public float damage; //공격력

    private bool isDead = false; //죽었는가?

    private bool isContact = false; //적을 만났는가?
    private Collider2D[] detectedEnemies = null; //만난 적

    private float startAttackTime; //공격을 시작한 시간
    public float attackInterval = 1f; //공격 주기

    public bool isRangeAttackType; //공격타입. true -> 범위공격, false -> 단일공격

    private float hpBarAmount { get { return hp / maxHp; } } //체력 비율

    public Image hpBar; //hp바 이미지

    public LayerMask TargetLayer; //타겟의 레이어를 꼭 지정해야함

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

    //체크하는 추상함수 -> 자식에서 구현을 해야함
    protected abstract Collider2D[] CheckEnemy();

    protected void Move()
    {
        //왼쪽으로 가는게 켜져있으면
        if (goLeft)
        {
            //크기 조절
            transform.localScale = Vector3.one * Scale;
            //이동 및 방향 조절
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            
        }
    }
}
